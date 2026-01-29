using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Swagger_Demo.Utils;
using Swashbuckle.Application;

namespace Swagger_Demo.App_Start
{
    /// <summary>
    /// Configures Swagger (OpenAPI) generation and the Swagger UI for Web API.
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Registers Swagger generator and UI. Call from Application_Start.
        /// </summary>
        /// <param name="config">HttpConfiguration to configure.</param>
        public static void Register(HttpConfiguration config)
        {
            string xmlCommentsPath = GetXmlCommentsPath();

            config.EnableSwagger(
                swaggerConfig =>
                {
                    // Define a single document named "v1"
                    swaggerConfig.SingleApiVersion(
                        "v1",
                        "MyApi - Demo"
                    );

                    // Include XML comments
                    if (File.Exists(xmlCommentsPath))
                    {
                        swaggerConfig.IncludeXmlComments(xmlCommentsPath);
                    }

                    swaggerConfig.ApiKey("api_key")
                        .Description("JWT Authorization using API Key (Bearer {token})")
                        .Name("api_key") // name of the header
                        .In("header"); // send in header

                    swaggerConfig.OperationFilter<AddAuthHeaderHandler>();
                    swaggerConfig.OperationFilter<FileUploadOperationFilter>();
                })
            .EnableSwaggerUi(
                c => { c.EnableApiKeySupport("api_key", "header"); }
            );



        }
                  
        /// <summary>
        /// Returns the XML comments file path for this assembly.
        /// </summary>
        /// <returns>Full path to XML documentation file emitted by compiler.</returns>
        private static string GetXmlCommentsPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFileName = string.Format("{0}.xml", Assembly.GetExecutingAssembly().GetName().Name);
            return Path.Combine(baseDirectory, "bin", xmlFileName);
        }
    }
}