using DoctorAppointmentAPI.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace DoctorAppointmentAPI.Filters
{
    /// <summary>
    /// Global exception handler of the application
    /// </summary>
    public class GlobalExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Method that will be called for exception
        /// </summary>
        /// <param name="context"></param>
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred";

            if (context.Exception is BusinessException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }

            HttpResponseMessage response = context.Request.CreateResponse(
                statusCode,
                new
                {
                    error = message
                });

            context.Result = new ResponseMessageResult(response);
        }
    }
}
