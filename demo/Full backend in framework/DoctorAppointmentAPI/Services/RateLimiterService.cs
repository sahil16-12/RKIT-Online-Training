using DoctorAppointmentAPI.Models;
using System;
using System.Runtime.Caching;

namespace DoctorAppointmentAPI.Services
{
    /// <summary>
    /// Rate limiter using MemoryCache instead of ConcurrentDictionary
    /// </summary>
    public class RateLimiterService
    {
        private static readonly Lazy<RateLimiterService> _lazy =
            new Lazy<RateLimiterService>(() => new RateLimiterService());

        private readonly MemoryCache _cache = MemoryCache.Default;

        private RateLimiterService() { }

        public static RateLimiterService Instance => _lazy.Value;

        /// <summary>
        /// Record a failed attempt
        /// </summary>
        public void RecordFailedAttempt(string key, int windowSeconds)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            TBL02 attempt = _cache.Get(key) as TBL02;

            if (attempt == null)
            {
                attempt = new TBL02
                {
                    L02F01 = 1,                    // Count
                    L02F02 = DateTime.UtcNow      // FirstAttemptTime
                };
            }
            else
            {
                attempt.L02F01++; // Count++
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(windowSeconds)
            };

            _cache.Set(key, attempt, policy);
        }

        /// <summary>
        /// Clear attempts
        /// </summary>
        public void Clear(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            _cache.Remove(key);
        }

        /// <summary>
        /// Check block status
        /// </summary>
        public bool IsBlocked(string key, int maxAttempts, int windowSeconds, out int remainingSeconds)
        {
            remainingSeconds = 0;

            if (string.IsNullOrWhiteSpace(key))
                return false;

            TBL02 attempt = _cache.Get(key) as TBL02;

            if (attempt == null)
                return false;

            if (attempt.L02F01 >= maxAttempts)
            {
                remainingSeconds = GetRemainingBlockTime(key, windowSeconds);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get remaining time
        /// </summary>
        public int GetRemainingBlockTime(string key, int windowSeconds)
        {
            TBL02 attempt = _cache.Get(key) as TBL02;

            if (attempt == null)
                return 0;

            int age = (int)(DateTime.UtcNow - attempt.L02F02).TotalSeconds;
            int remaining = windowSeconds - age;

            return remaining > 0 ? remaining : 0;
        }
    }
}
