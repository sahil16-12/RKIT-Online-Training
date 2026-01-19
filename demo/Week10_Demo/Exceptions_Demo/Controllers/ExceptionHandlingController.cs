using System;
using System.Net;
using System.Web.Http;

namespace Exceptions_Demo.Controllers
{

    public class ExceptionHandlingController : ApiController
    {
        [HttpGet]
        [Route("api/testerror")]
        public IHttpActionResult TestException()
        {
            // This will be caught by our global handler
            throw new InvalidOperationException("Something bad happened!");
        }

        [HttpGet]
        [Route("api/specificerror")]
        public IHttpActionResult Specific404(int id)
        {
            if (id <= 0)
                return Content(HttpStatusCode.BadRequest, "Invalid ID");

            return Ok("Success!");
        }
    }

}