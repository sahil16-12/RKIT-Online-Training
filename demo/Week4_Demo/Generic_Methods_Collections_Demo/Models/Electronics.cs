using System;

namespace Generic_Methods_Collections_Demo.Models
{
    /// <summary>
    /// Represents an electronic item stored in the warehouse.
    /// </summary>
    public class Electronics : Product
    {
        /// <summary>
        /// Warranty period in years.
        /// </summary>
        public int WarrantyYears { get; set; }

        /// <summary>
        /// Displays electronic product information.
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"[Electronics] ID: {Id}, Name: {Name}, Price: ₹{Price}, Warranty: {WarrantyYears} years");
        }
    }
}

