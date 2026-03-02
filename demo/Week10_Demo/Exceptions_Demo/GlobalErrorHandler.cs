using Exceptions_Demo.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Exceptions_Demo
{
    /// <summary>
    /// Global exception handler. This is the final fallback for unhandled exceptions in the pipeline.
    /// It converts exceptions to a consistent JSON response.
    /// </summary>
    public class GlobalErrorHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            string detailMessage = context.Exception.Message;

            // If exception is BusinessException, allow a specific status code and message.
            BusinessException businessException = context.Exception as BusinessException;
            if (businessException != null)
            {
                statusCode = businessException.StatusCode;
                message = businessException.Message;
                detailMessage = businessException.Detail;
            }

            ErrorResponse payload = new ErrorResponse
            {
                Error = message,
                Detail = detailMessage
            };

            HttpResponseMessage response = context.Request.CreateResponse(statusCode, payload);
            context.Result = new ResponseMessageResult(response);
        }
    }
}
