using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
namespace Exceptions_Demo
{
    /// <summary>
    /// Web API configuration and services.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register Web API configuration.
        /// Call this from Global.asax
        /// </summary>
        /// <param name="config">HttpConfiguration instance</param>
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Register global exception handler
            config.Services.Replace(
                typeof(IExceptionHandler),
                new GlobalErrorHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}
