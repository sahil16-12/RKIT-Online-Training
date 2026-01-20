using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

/// <summary>
/// WEB Api configuration
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
        // Enable attribute routing
        config.MapHttpAttributeRoutes();

        // Enable CORS globally
        var cors = new EnableCorsAttribute("*", "*", "*");
        config.EnableCors(cors);

        // Default API route
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}

