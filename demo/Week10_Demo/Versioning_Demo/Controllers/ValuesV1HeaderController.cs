using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Versioning_Demo.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/values")]
    public class ValuesV1HeaderController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new[] { "v1-value1", "v1-value2" });
        }
    }
}