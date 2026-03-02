using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Caching_Demo
{
    /// <summary>
    /// Web API configuration (routes, etc.)
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register routes and configuration.
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing
            config.MapHttpAttributeRoutes();

            // Default conventional route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
