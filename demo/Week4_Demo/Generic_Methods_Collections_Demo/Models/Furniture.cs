using System;

namespace Generic_Methods_Collections_Demo.Models
{
    /// <summary>
    /// Represents a furniture item stored in the warehouse.
    /// </summary>
    public class Furniture : Product
    {
        /// <summary>
        /// Type of material used (e.g., Wood, Metal).
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// Displays furniture details.
        /// </summary>
        public override void DisplayInfo()
        {
            Console.WriteLine($"[Furniture] ID: {Id}, Name: {Name}, Price: ₹{Price}, Material: {Material}");
        }
    }
}
