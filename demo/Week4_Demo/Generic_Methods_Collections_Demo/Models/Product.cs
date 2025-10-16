using System;

namespace Generic_Methods_Collections_Demo.Models
{
    /// <summary>
    /// Represents a basic product in the warehouse.
    /// This will act as a base class for all product types.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique product ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Displays product information.
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Price: ₹{Price}");
        }
    }
}
