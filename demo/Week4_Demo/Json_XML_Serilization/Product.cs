using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonXmlDemo
{
    /// <summary>
    /// Simple Product class for serialization demo
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
