using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentAPI.DTOs.Appointments
{
    /// <summary>
    /// Appointment request DTO
    /// </summary>
    public class CreateAppointmentRequestDto
    {
        /// <summary>
        /// Represents the doctor ID
        /// </summary>
        [Required]
        public int DoctorId { get; set; }

        /// <summary>
        /// Represents the Appointment date
        /// </summary>
        [Required]
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Represents reason for appointment
        /// </summary>
        public string Reason { get; set; }
    }
}
