using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Caching_Demo.Controllers
{

    /// <summary>
    /// API controller for product endpoints with caching.
    /// </summary>
    public class ProductsController : ApiController
    {
        /// <summary>
        /// GET api/products — returns product list with a flag indicating source.
        /// </summary>
        [HttpGet]
        [Route("api/products")]
        public IHttpActionResult Get()
        {
            bool fromCache;

            var products = CacheHelper.GetOrSet(
                "products",                         // cache key
                () => FetchProductsFromDatabase(), // delegate to fetch if cache miss
                TimeSpan.FromMinutes(5),           // cache duration
                out fromCache                      // out param to know source
            );

            string source = fromCache ? "cache" : "database";

            return Ok(new { source, products });
        }

        /// <summary>
        /// Simulates a database call.
        /// </summary>
        /// <returns>A list of product names.</returns>
        private List<string> FetchProductsFromDatabase()
        {
            // Imagine this calls your database with a heavy query.
            return new List<string> { "Laptop", "Phone", "Tablet" };
        }
    }


}