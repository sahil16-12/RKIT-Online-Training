using System.Collections.Generic;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace Swagger_Demo.Utils
{
    public class AddAuthHeaderHandler : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                required = false,
                description = "JWT token (Bearer {token})"
            });
        }
    }
}