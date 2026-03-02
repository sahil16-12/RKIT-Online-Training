using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace backend.Controllers
{
    /// <summary>
    /// Exposes authentication APIs for signup and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Private Fields

        /// <summary>
        /// Represents the authentication service instance.
        /// </summary>
        private readonly IAuthService _authService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The signup request payload.</param>
        /// <returns>An HTTP response indicating signup result.</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            await _authService.PreSaveAsync(request);
            await _authService.ValidateAsync();
            await _authService.SaveAsync();

            return StatusCode(201, new { message = "User registered successfully." });
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="request">The login request payload.</param>
        /// <returns>An HTTP response containing token and profile on success.</returns>

        [EnableRateLimiting("login-policy")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse response = await _authService.LoginAsync(request);

            return Ok(response);
        }

        #endregion
    }
}
