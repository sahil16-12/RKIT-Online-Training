using System;
using System.Net;
using System.Web.Http;

namespace Exceptions_Demo.Controllers
{
    /// <summary>
    /// Controller used to demonstrate exception handling in Web API.
    /// Contains endpoints that throw exceptions or return specific HTTP errors
    /// for testing global and local exception handling mechanisms.
    /// </summary>
    public class ExceptionHandlingController : ApiController
    {
        /// <summary>
        /// Test endpoint that deliberately throws an exception.
        /// This exception is intended to be caught by a global exception handler
        /// </summary>
        /// <returns>
        /// This method never returns a successful response because it always throws an exception.
        /// </returns>
        [HttpGet]
        [Route("api/testerror")]
        public IHttpActionResult TestException()
        {
            // This exception will be caught by the global exception handler
            throw new InvalidOperationException("Something bad happened!");
        }

        /// <summary>
        /// Demonstrates returning a specific HTTP status code based on input validation.
        /// Returns 400 BadRequest if the provided ID is invalid, otherwise returns success.
        /// </summary>
        /// <param name="id">
        /// The ID to validate. Must be greater than zero.
        /// </param>
        /// <returns>
        /// 400 BadRequest if ID is invalid, or 200 OK if valid.
        /// </returns>
        [HttpGet]
        [Route("api/specificerror")]
        public IHttpActionResult Specific404(int id)
        {
            // Validate input
            if (id <= 0)
                return Content(HttpStatusCode.BadRequest, "Invalid ID");

            // Return success if ID is valid
            return Ok("Success!");
        }
    }
}
