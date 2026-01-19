using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
namespace Exceptions_Demo
{

    public class GlobalErrorHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var errorResponse = new
            {
                Message = "Sorry, something went wrong.",
                Detail = context.Exception.Message
            };

            context.Result = new ResponseMessageResult(
                context.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    errorResponse));
        }
    }

}