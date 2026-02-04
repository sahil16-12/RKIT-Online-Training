using System;

namespace DoctorAppointmentAPI.DTOs.Appointments
{
    /// <summary>
    /// Appointments response DTO
    /// </summary>
    public class AppointmentResponseDto
    {
        /// <summary>
        /// Represents appointment Id
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// Represents doctor Id
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Represents patient Id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Represents appointment date
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Represents status of appointment
        /// </summary>
        public string Status { get; set; }
    }
}
