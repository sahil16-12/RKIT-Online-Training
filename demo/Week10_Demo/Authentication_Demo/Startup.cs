using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Text;
using System.Web.Http;

[assembly: OwinStartup(typeof(Authentication_Demo.Startup))]

namespace Authentication_Demo
{
    /// <summary>
    /// Startup class for configuring OWIN middleware pipeline.
    /// This class initializes Web API and configures JWT authentication.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the OWIN application pipeline.
        /// Registers Web API routes and enables JWT Bearer authentication
        /// for securing API endpoints.
        /// </summary>
        /// <param name="app">
        /// The OWIN application builder used to register middleware components.
        /// </param>
        public void Configuration(IAppBuilder app)
        {
            // Create HttpConfiguration for Web API
            var config = new HttpConfiguration();

            // Register Web API routes
            WebApiConfig.Register(config);

            // Secret key used to validate JWT token signatures
            var key = Encoding.UTF8.GetBytes("super_secret_key_12345678901234567890");

            // Configure JWT Bearer authentication middleware
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                // Active mode means authentication happens automatically for each request
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,

                // Parameters used to validate incoming JWT tokens
                TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate that the token was signed with the correct key
                    ValidateIssuerSigningKey = true,

                    // Validate token expiration time
                    ValidateLifetime = true,

                    // The symmetric key used to validate token signature
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }
            });

            // Attach Web API to OWIN pipeline
            app.UseWebApi(config);
        }
    }
}
