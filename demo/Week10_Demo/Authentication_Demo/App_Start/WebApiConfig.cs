using Authentication_Demo.App_Start;
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
        config.SuppressDefaultHostAuthentication();
        config.Filters.Add(new HostAuthenticationFilter("Bearer"));
        // Enable attribute routing
        config.MapHttpAttributeRoutes();

        // Enable CORS globally
        EnableCorsAttribute cors = new EnableCorsAttribute("http://127.0.0.1:5500", "*", "*");
        config.EnableCors(cors);

        // Default API route
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );

    }
}

