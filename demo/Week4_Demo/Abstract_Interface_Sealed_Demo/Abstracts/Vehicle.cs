using Abstract_Interface_Sealed_Demo.Interfaces;
using System;

namespace Abstract_Interface_Sealed_Demo.Abstracts
{
    /// <summary>
    /// Abstract base class that represents a general vehicle.
    /// It provides common properties and partial implementations for all vehicle types.
    /// </summary>
    public abstract class Vehicle : IVehicle
    {
        /// <summary>
        /// Unique registration number for the vehicle.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Brand name of the vehicle.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Maximum speed (in km/h) that the vehicle can reach.
        /// </summary>
        public int MaxSpeed { get; set; }

        /// <summary>
        /// Constructor to initialize common vehicle properties.
        /// </summary>
        public Vehicle(string registrationNumber, string brand, int maxSpeed)
        {
            RegistrationNumber = registrationNumber;
            Brand = brand;
            MaxSpeed = maxSpeed;
        }

        /// <summary>
        /// Starts the vehicle. Abstract method forces child classes to implement their own logic.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the vehicle. Abstract method forces child classes to implement their own logic.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Displays vehicle details. Can be overridden by derived classes.
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Brand: {Brand}, Registration: {RegistrationNumber}, Max Speed: {MaxSpeed} km/h");
        }
    }
}
