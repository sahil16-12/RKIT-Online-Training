using DoctorAppointmentAPI.Filters;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DoctorAppointmentAPI
{
    /// <summary>
    /// Web API configuration class for routing and CORS setup
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers Web API configuration including routes and CORS
        /// </summary>
        /// <param name="config">HTTP configuration object</param>
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS for all origins, headers, and methods
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            ////Configuring auto mapper
            //AutoMapperConfig.Register();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Services.Replace(
                    typeof(System.Web.Http.ExceptionHandling.IExceptionHandler),
                    new GlobalExceptionHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}