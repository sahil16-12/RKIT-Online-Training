using System;

namespace Abstract_Interface_Sealed_Demo.Interfaces
{
    /// <summary>
    /// Represents the basic contract for any vehicle in the rental system.
    /// It ensures that all vehicle types implement Start, Stop, and DisplayInfo methods.
    /// </summary>
    public interface IVehicle
    {
        /// <summary>
        /// Starts the vehicle.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the vehicle.
        /// </summary>
        void Stop();

        /// <summary>
        /// Displays key details about the vehicle.
        /// </summary>
        void DisplayInfo();
    }
}
