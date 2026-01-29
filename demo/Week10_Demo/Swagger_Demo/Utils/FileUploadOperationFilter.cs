using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swagger_Demo.Utils
{
    /// <summary>
    /// Operation filter to add file upload parameter to Swagger UI for image upload endpoints.
    /// This enables the "Choose File" button in the Swagger interface.
    /// </summary>
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // Check if this is the file upload endpoint
            if (apiDescription.RelativePath == "api/image/upload")
            {
                // Clear any existing parameters
                operation.parameters = operation.parameters ?? new List<Parameter>();

                // Remove the Authorization parameter temporarily (we'll add it back)
                var authParam = operation.parameters.FirstOrDefault(p => p.name == "Authorization");

                // Clear all parameters
                operation.parameters.Clear();

                // Add the file upload parameter
                operation.parameters.Add(new Parameter
                {
                    name = "file",
                    @in = "formData",
                    description = "Image file to upload (JPG, PNG, GIF, BMP - Max 10MB)",
                    required = true,
                    type = "file"
                });

                // Add back the Authorization parameter if it existed
                if (authParam != null)
                {
                    operation.parameters.Add(authParam);
                }

                // Set the consumes type
                operation.consumes = new List<string> { "multipart/form-data" };
            }
        }
    }
}
