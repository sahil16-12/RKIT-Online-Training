using Authentication_Demo.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Authentication_Demo.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related operations.
    /// Provides APIs for user login and JWT token generation.
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// Authenticates the user using username and password.
        /// If credentials are valid, generates and returns a JWT token.
        /// </summary>
        /// <param name="model">
        /// Login model containing Username and Password provided by the client.
        /// </param>
        /// <returns>
        /// Returns 200 OK with JWT token if authentication succeeds,
        /// otherwise returns 401 Unauthorized.
        /// </returns>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginModel model)
        {
            // Hardcoded demo user validation
            if (model.Username != "admin" || model.Password != "password")
                return Unauthorized();

            // Token handler used to create and write JWT tokens
            var tokenHandler = new JwtSecurityTokenHandler();

            // Secret key used to sign the JWT token (should be stored securely in real apps)
            var key = Encoding.UTF8.GetBytes("super_secret_key_12345678901234567890");

            // Describe the token: claims, expiry, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // Username claim
                    new Claim(ClaimTypes.Name, model.Username),

                    // Role claim (used for authorization)
                    new Claim(ClaimTypes.Role, "Admin")
                }),

                // Token expiration time
                Expires = DateTime.UtcNow.AddMinutes(30),

                // Token signing configuration
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return token to client
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
