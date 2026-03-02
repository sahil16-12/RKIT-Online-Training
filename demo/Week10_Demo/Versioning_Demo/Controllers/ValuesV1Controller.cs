using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Versioning_Demo.Controllers
{
    /// <summary>
    /// Version 1.0 of Values API.
    /// This controller responds to routes such as:
    ///  - GET /api/v1/values   (URL segment)
    ///  - GET /api/values      (if default version applied)
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]
    public class ValuesV1Controller : ApiController
    {
        /// <summary>
        /// GET api/v{version}/values
        /// Returns a simple list of strings for v1.
        /// </summary>
        /// <returns>List of string values.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            List<string> values = new List<string> { "value1", "value2-from-v1" };
            return Ok(values);
        }
    }
}