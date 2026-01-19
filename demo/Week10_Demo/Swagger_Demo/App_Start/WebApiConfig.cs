using Swagger_Demo.App_Start;
using System.Web.Http;

namespace Swagger_Demo
{
    /// <summary>
    /// Standard Web API routing configuration.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register routing and other Web API configuration.
        /// </summary>
        /// <param name="config">HttpConfiguration instance.</param>
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing
            config.MapHttpAttributeRoutes();

            // Default route (conventional)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register Swagger after routes are configured
            SwaggerConfig.Register(config);
        }
    }
}
