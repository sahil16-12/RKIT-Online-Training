using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Versioning_Demo.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/values")]
    public class ValuesV2HeaderController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new[] { "v2-value1", "v2-value2", "v2-value3" });
        }
    }

}