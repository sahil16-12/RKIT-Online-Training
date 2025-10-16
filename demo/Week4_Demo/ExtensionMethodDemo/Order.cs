using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethodDemo
{
    /// <summary>
    /// Simple order class
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
    }
}
