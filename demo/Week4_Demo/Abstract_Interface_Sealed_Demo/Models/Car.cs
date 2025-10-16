using System;
using Abstract_Interface_Sealed_Demo.Abstracts;

namespace Abstract_Interface_Sealed_Demo.Models
{
    /// <summary>
    /// Represents a car that can be rented.
    /// Inherits from Vehicle and implements abstract methods.
    /// </summary>
    public class Car : Vehicle
    {
        /// <summary>
        /// Total number of seats available in the car.
        /// </summary>
        public int NumberOfSeats { get; set; }

        /// <summary>
        /// Initializes a new instance of the Car class.
        /// </summary>
        public Car(string registrationNumber, string brand, int maxSpeed, int seats)
            : base(registrationNumber, brand, maxSpeed)
        {
            NumberOfSeats = seats;
        }

        /// <summary>
        /// Starts the car. Demonstrates engine ignition.
        /// </summary>
        public override void Start()
        {
            Console.WriteLine($"Car {Brand} with registration {RegistrationNumber} started.");
        }

        /// <summary>
        /// Stops the car. Demonstrates turning off the engine.
        /// </summary>
        public override void Stop()
        {
            Console.WriteLine($"Car {Brand} stopped.");
        }

        /// <summary>
        /// Displays all car details including seats and base info.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Seats: {NumberOfSeats}");
        }
    }
}
