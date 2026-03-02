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
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var key = Encoding.UTF8.GetBytes("super_secret_key_12345678901234567890");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,

                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                }
            });

            app.UseWebApi(config);
        }
    }
}
