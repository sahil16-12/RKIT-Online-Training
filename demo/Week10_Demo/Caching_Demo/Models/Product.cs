using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caching_Demo.Models
{
    /// <summary>
    /// Simple product model.
    /// </summary>
    public class Product
    {
        /// <summary>Product identifier.</summary>
        public int Id { get; set; }

        /// <summary>Product name.</summary>
        public string Name { get; set; }

        /// <summary>Product price in rupees</summary>
        public decimal Price { get; set; }
    }
}