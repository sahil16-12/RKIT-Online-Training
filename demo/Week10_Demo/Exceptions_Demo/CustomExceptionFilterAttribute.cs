using Exceptions_Demo.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Exceptions_Demo
{
    /// <summary>
    /// Exception filter runs after an action throws (before GlobalErrorHandler).
    /// Use it to transform or short-circuit certain exceptions
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is System.ComponentModel.DataAnnotations.ValidationException)
            {
                ErrorResponse payload = new ErrorResponse
                {
                    Error = "Validation failed",
                    Detail = context.Exception.Message
                };
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, payload);
                return;
            }

            if (context.Exception is BusinessException)
            {
                BusinessException businessException = (BusinessException)context.Exception;
                ErrorResponse payload = new ErrorResponse
                {
                    Error = businessException.Message,
                    Detail = businessException.Detail
                };
                context.Response = context.Request.CreateResponse(businessException.StatusCode, payload);
                return;
            }

            // For other exceptions: let GlobalErrorHandler handle them
            base.OnException(context);
        }
    }
}