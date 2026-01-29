using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace Caching_Demo.Caching
{
    /// <summary>
    /// Simple thread-safe MemoryCache based cache-aside helper.
    /// Uses a per-key lock to avoid stampede (many concurrent fetches).
    /// </summary>
    public static class MemoryCacheService
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private static readonly ConcurrentDictionary<string, object> KeyLocks = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Get value from memory cache or load it using fetch delegate and cache it.
        /// </summary>
        /// <typeparam name="T">Type of cached object.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="fetch">Delegate that loads data on cache miss.</param>
        /// <param name="duration">Absolute TTL to cache value.</param>
        /// <returns>Cached or fetched value.</returns>
        public static T GetOrSet<T>(string key, Func<T> fetch, TimeSpan duration)
        {
            object existing = Cache.Get(key);
            if (existing != null)
            {
                return (T)existing;
            }

            // Per-key lock to prevent thundering herd
            object myLock = KeyLocks.GetOrAdd(key, k => new object());
            lock (myLock)
            {
                // Check again inside lock
                object existingInside = Cache.Get(key);
                if (existingInside != null)
                {
                    return (T)existingInside;
                }

                // Load and cache
                T value = fetch();
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(duration)
                };
                Cache.Set(key, value, policy);

                // Remove lock
                object removed;
                KeyLocks.TryRemove(key, out removed);

                return value;
            }
        }

        /// <summary>
        /// Remove a cache key manually.
        /// </summary>
        /// <param name="key">Cache key to remove.</param>
        public static void Remove(string key)
        {
            if (Cache.Contains(key))
            {
                Cache.Remove(key);
            }
        }
    }
}