using Caching_Demo.Caching;
using Caching_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace Caching_Demo.Controllers
{

    /// <summary>
    /// Products demo controller — demonstrates client caching, memory cache.
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        // NOTE: Replace with repository / DB calls
        private static readonly List<Product> SampleProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 75000M },
            new Product { Id = 2, Name = "Phone", Price = 25000M },
            new Product { Id = 3, Name = "Tablet", Price = 30000M }
        };

        /// <summary>
        /// GET api/products/client
        /// Demonstrates client-side caching using Cache-Control and ETag.
        /// Browser/clients can cache and use conditional GET with If-None-Match.
        /// </summary>
        [HttpGet]
        [Route("client")]
        public IHttpActionResult GetForClientCaching()
        {
            // Simulate fetching resource (DB)
            List<Product> products = GetProductsFromDataSource();

            // Compute ETag from content
            string etag = ETagHelper.ComputeETag(products);

            // If client sent If-None-Match and it matches, return 304
            string ifNoneMatch = Request.Headers.IfNoneMatch?.FirstOrDefault()?.Tag;
            if (!string.IsNullOrEmpty(ifNoneMatch) && ifNoneMatch == etag)
            {
                HttpResponseMessage notModified = new HttpResponseMessage(HttpStatusCode.NotModified);
                notModified.Headers.ETag = new System.Net.Http.Headers.EntityTagHeaderValue(etag);
                return ResponseMessage(notModified);
            }

            // Build response with Cache-Control and ETag
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, products);
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(60) // client & proxy can cache for 60s
            };
            response.Headers.ETag = new System.Net.Http.Headers.EntityTagHeaderValue(etag);

            return ResponseMessage(response);
        }

        /// <summary>
        /// GET api/products/memory
        /// Demonstrates in-memory cache using MemoryCacheService; TTL 30 seconds
        /// </summary>
        [HttpGet]
        [Route("memory")]
        public IHttpActionResult GetWithMemoryCache()
        {
            List<Product> products = MemoryCacheService.GetOrSet(
                "products_all",
                () => GetProductsFromDataSource(),
                TimeSpan.FromSeconds(30)
            );

            return Ok(new { source = "memory", products = products, timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// GET api/products/evict/{store}
        /// Helper to evict cache manually.
        /// </summary>
        [HttpGet]
        [Route("evict/{store}")]
        public IHttpActionResult Evict(string store)
        {
            if (string.Equals(store, "memory", StringComparison.OrdinalIgnoreCase))
            {
                MemoryCacheService.Remove("products_all");
                return Ok(new { evicted = "memory" });
            }

            return BadRequest("Unknown store. Use 'memory'.");
        }

        /// <summary>
        /// Simulated data source fetch. In real app, call DB or external service.
        /// Also writes to server console to indicate a DB hit for visibility.
        /// </summary>
        /// <returns>List of products.</returns>
        private static List<Product> GetProductsFromDataSource()
        {
            // In demo, we log to Output window so you can see when DB is hit.
            System.Diagnostics.Debug.WriteLine("GetProductsFromDataSource called at " + DateTime.UtcNow.ToString("o"));
            return SampleProducts.Select(p => new Product { Id = p.Id, Name = p.Name, Price = p.Price }).ToList();
        }
    }


}