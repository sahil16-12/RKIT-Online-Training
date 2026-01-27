namespace LoginRateLimiterDemo.Models
{
    /// <summary>
    /// Result of rate limiting check.
    /// </summary>
    public class RateLimitResult
    {
        public bool IsBlocked { get; set; }
        public int Attempts { get; set; }
        public string Message { get; set; }
    }
}
