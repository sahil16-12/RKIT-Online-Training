using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.IdentityModel.Tokens;
using Owin;
using System.Text;
using System.Configuration;

[assembly: OwinStartup(typeof(DoctorAppointmentAPI.Startup))]

namespace DoctorAppointmentAPI
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration method
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureJwt(app);
        }

        /// <summary>
        /// JWT configuration 
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureJwt(IAppBuilder app)
        {
            string secretKey = ConfigurationManager.AppSettings["JwtSecret"];

            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,

                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                }
            });
        }
    }
}
