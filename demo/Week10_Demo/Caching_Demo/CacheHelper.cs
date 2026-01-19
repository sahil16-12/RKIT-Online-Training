using System;
using System.Runtime.Caching;

namespace Caching_Demo
{
    /// <summary>
    /// Provides in-memory caching functionality using MemoryCache.
    /// </summary>
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Provides in-memory caching functionality using MemoryCache.
    /// </summary>
    public static class CacheHelper
    {
        private static MemoryCache _cache = MemoryCache.Default;

        /// <summary>
        /// Retrieves cached data by key or uses fetch delegate to get and cache it.
        /// </summary>
        /// <typeparam name="T">Type of cached data.</typeparam>
        /// <param name="key">Cache key.</param>
        /// <param name="fetch">Delegate to fetch data if cache miss.</param>
        /// <param name="duration">How long to cache the data.</param>
        /// <param name="fromCache">Output parameter indicating cache hit/miss.</param>
        /// <returns>The cached or fetched value.</returns>
        public static T GetOrSet<T>(string key, Func<T> fetch, TimeSpan duration, out bool fromCache)
        {
            if (_cache.Contains(key))
            {
                fromCache = true;
                return (T)_cache.Get(key);
            }

            fromCache = false;
            T data = fetch();

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(duration)
            };

            _cache.Set(key, data, policy);
            return data;
        }
    }


}