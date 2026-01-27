using LoginRateLimiterDemo.Models;
using LoginRateLimiterDemo.Services;
using System.Web.Http;

namespace LoginRateLimiterDemo.Controllers
{
    /// <summary>
    /// Simulates login API with Redis rate limiting.
    /// </summary>
    public class AuthController : ApiController
    {
        private readonly RateLimiterService _rateLimiter;

        public AuthController()
        {
            _rateLimiter = new RateLimiterService();
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login(string userId)
        {
            RateLimitResult result = _rateLimiter.CheckLoginAttempts(userId);
            System.Console.WriteLine("Hello sahil");

            if (result.IsBlocked)
            {
                return Content((System.Net.HttpStatusCode)429, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/ping")]
        public string Ping()
        {
            return "OK";
        }

    }
}
