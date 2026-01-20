using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Exceptions_Demo
{
    /// <summary>
    /// Global exception handler for the Web API application.
    /// This handler catches all unhandled exceptions and returns
    /// a standardized error response to the client.
    /// </summary>
    public class GlobalErrorHandler : ExceptionHandler
    {
        /// <summary>
        /// Handles unhandled exceptions that occur during request processing.
        /// Converts the exception into a consistent HTTP 500 error response
        /// with a user-friendly message and technical details.
        /// </summary>
        /// <param name="context">
        /// The context that contains the exception, request, and response information.
        /// </param>
        public override void Handle(ExceptionHandlerContext context)
        {
            // Create a standard error response object
            var errorResponse = new
            {
                Message = "Sorry, something went wrong.",
                Detail = context.Exception.Message
            };

            // Set the HTTP response with status code 500 (Internal Server Error)
            context.Result = new ResponseMessageResult(
                context.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    errorResponse
                )
            );
        }
    }
}
