using Swashbuckle.Application;
using System.IO;
using System;
using System.Web.Http;

namespace DoctorAppointmentAPI
{
    /// <summary>
    /// Swagger configuration
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Swagger configuration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                // API basic info
                c.SingleApiVersion("MyAPI", "Doctor Appointment Booking API");

                var xmlPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "bin",
                    "DoctorAppointmentAPI.xml"
                );
                c.IncludeXmlComments(xmlPath);

                // JWT Bearer token support
                c.ApiKey("Bearer")
                 .Description("JWT Authorization header. Example: \"Bearer {token}\"")
                 .Name("Authorization")
                 .In("header");
            })
            .EnableSwaggerUi(c =>
            {
                // Enables the Authorize input box
                c.EnableApiKeySupport("Authorization", "header");
            });
        }
    }
}
