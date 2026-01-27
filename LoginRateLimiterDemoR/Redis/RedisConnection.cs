using StackExchange.Redis;
using System;

namespace LoginRateLimiterDemo.Redis
{
    /// <summary>
    /// Manages Redis connection.
    /// </summary>
    public static class RedisConnection
    {
        private static readonly Lazy<ConnectionMultiplexer> _connection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                ConfigurationOptions options = new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" },
                    AbortOnConnectFail = false,
                    ConnectRetry = 3,
                    ConnectTimeout = 5000
                };

                return ConnectionMultiplexer.Connect(options);
            });

        /// <summary>
        /// Gets Redis database instance.
        /// </summary>
        public static IDatabase Database
        {
            get { return _connection.Value.GetDatabase(); }
        }
    }
}
