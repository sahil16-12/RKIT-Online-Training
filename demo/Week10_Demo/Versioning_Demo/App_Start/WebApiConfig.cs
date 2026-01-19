using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;

namespace Versioning_Demo
{
    /// <summary>
    /// Web API configuration and services.
    /// Registers routes and API versioning.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register Web API configuration.
        /// Call this from Global.asax or Startup.
        /// </summary>
        /// <param name="config">HttpConfiguration instance</param>
        public static void Register(HttpConfiguration config)
        {
            // Create inline constraint resolver
            DefaultInlineConstraintResolver constraintResolver =
                new DefaultInlineConstraintResolver();

            // Register apiVersion constraint
            constraintResolver.ConstraintMap.Add(
                "apiVersion",
                typeof(ApiVersionRouteConstraint)
            );

            // Use custom resolver
            config.MapHttpAttributeRoutes(constraintResolver);

            // Configure API versioning
            config.AddApiVersioning(options =>
            {
                // Report the supported versions in response headers
                options.ReportApiVersions = true;

                // Use 1.0 as the default when no version is provided
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 0);

                // Accept version from URL segment OR from custom header "x-api-version"
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("x-api-version"),
                    new UrlSegmentApiVersionReader()

                );
            });


            // Default fallback route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
