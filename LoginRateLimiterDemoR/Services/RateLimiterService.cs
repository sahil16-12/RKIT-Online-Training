using LoginRateLimiterDemo.Models;
using LoginRateLimiterDemo.Redis;
using StackExchange.Redis;
using System;

namespace LoginRateLimiterDemo.Services
{
    /// <summary>
    /// Handles login rate limiting using Redis.
    /// </summary>
    public class RateLimiterService
    {
        private const int MAX_ATTEMPTS = 5;
        private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Checks and increments login attempts for a user.
        /// </summary>
        public RateLimitResult CheckLoginAttempts(string userId)
        {
            IDatabase db = RedisConnection.Database;
            string key = "login:attempts:" + userId;

            long attempts = db.StringIncrement(key);

            if (attempts == 1)
            {
                db.KeyExpire(key, WINDOW);
            }

            if (attempts > MAX_ATTEMPTS)
            {
                return new RateLimitResult
                {
                    IsBlocked = true,
                    Attempts = (int)attempts,
                    Message = "Too many login attempts. Please try again later."
                };
            }

            return new RateLimitResult
            {
                IsBlocked = false,
                Attempts = (int)attempts,
                Message = "Login attempt allowed."
            };
        }
    }
}
