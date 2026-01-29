using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
namespace Exceptions_Demo
{
    /// <summary>
    /// Web API configuration: routes, global filters, message handlers and global exception handler replacement.
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing
            config.MapHttpAttributeRoutes();

            // Add a message handler (runs for every request)
            config.MessageHandlers.Add(new LoggingHandler());

            // Add an exception filter (can short-circuit controller exceptions)
            config.Filters.Add(new CustomExceptionFilterAttribute());

            // Replace the default global exception handler
            config.Services.Replace(
                typeof(System.Web.Http.ExceptionHandling.IExceptionHandler),
                new GlobalErrorHandler());

            // Default route (optional — attribute routing is preferred)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}
