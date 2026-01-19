using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Swagger_Demo.Filters
{
    /// <summary>
    /// Adds the Authorization header parameter to each operation in the generated Swagger doc.
    /// This makes it possible to enter a Bearer token in the UI for testing protected endpoints.
    /// </summary>
    public class AddAuthHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to an operation.
        /// </summary>
        /// <param name="operation">The operation being generated.</param>
        /// <param name="schemaRegistry">The schema registry.</param>
        /// <param name="apiDescription">API description for the operation.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                required = false,
                description = "Bearer {token} - enter without quotes"
            });
        }
    }
}