using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Swagger_Demo.Controllers
{
    /// <summary>
    /// Demonstration controller — basic CRUD-like endpoints for values.
    /// </summary>
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        /// <summary>
        /// GET api/values
        /// Returns a small list of values.
        /// </summary>
        /// <returns>List of string values.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            List<string> values = new List<string>
            {
                "value1",
                "value2",
                "value3"
            };

            return Ok(values);
        }

        /// <summary>
        /// GET api/values/{id}
        /// Returns the requested value by id.
        /// </summary>
        /// <param name="id">Index of the value to return.</param>
        /// <returns>Single value string or NotFound.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            if (id < 1 || id > 3)
            {
                return NotFound();
            }

            string value = "value" + id;
            return Ok(value);
        }
    }
}