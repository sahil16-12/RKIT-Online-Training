using System;
using Abstract_Interface_Sealed_Demo.Abstracts;

namespace Abstract_Interface_Sealed_Demo.Models
{
    /// <summary>
    /// Represents a bike that can be rented.
    /// This class is sealed, meaning no other class can inherit from it.
    /// </summary>
    public sealed class Bike : Vehicle
    {
        /// <summary>
        /// Type of bike (e.g., Sports, Cruiser, Standard).
        /// </summary>
        public string Type { get; set; }

        public Bike(string registrationNumber, string brand, int maxSpeed, string type)
            : base(registrationNumber, brand, maxSpeed)
        {
            Type = type;
        }

        /// <summary>
        /// Starts the bike.
        /// </summary>
        public override void Start()
        {
            Console.WriteLine($"Bike {Brand} ({Type}) started.");
        }

        /// <summary>
        /// Stops the bike.
        /// </summary>
        public override void Stop()
        {
            Console.WriteLine($"Bike {Brand} stopped.");
        }

        /// <summary>
        /// Displays all bike details.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Bike Type: {Type}");
        }
    }
}

