using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Versioning_Demo.Controllers
{
    /// <summary>
    /// Version 2.0 of Values API.
    /// This controller responds to routes such as:
    ///  - GET /api/v2/values   (URL segment)
    ///  - GET /api/values with header x-api-version: 2
    /// </summary>
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]
    public class ValuesV2Controller : ApiController
    {
        /// <summary>
        /// GET api/v{version}/values
        /// Returns extended values for v2.
        /// </summary>
        /// <returns>List of strings with v2 marker.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            List<string> values = new List<string> { "value1", "value2-from-v2", "value3-new" };
            return Ok(values);
        }
    }
}