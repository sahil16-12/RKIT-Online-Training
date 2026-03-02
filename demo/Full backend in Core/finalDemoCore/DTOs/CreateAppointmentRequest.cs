using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.DTOs
{
    /// <summary>
    /// Represents patient request for creating a new appointment.
    /// </summary>
    public class CreateAppointmentRequest
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets doctor user identifier.
        /// </summary>
        [Required(ErrorMessage = "Please select a doctor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor.")]
        [JsonPropertyName("DoctorUserId")]
        public int L04F03 { get; set; }

        /// <summary>
        /// Gets or sets appointment UTC date and time.
        /// </summary>
        [Required(ErrorMessage = "Appointment date and time is required.")]
        [JsonPropertyName("AppointmentAtUtc")]
        public DateTime L04F04 { get; set; }

        /// <summary>
        /// Gets or sets reason for booking.
        /// </summary>
        [Required(ErrorMessage = "Reason is required.")]
        [MinLength(5, ErrorMessage = "Reason must be at least 5 characters.")]
        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        [JsonPropertyName("Reason")]
        public string L04F05 { get; set; } = string.Empty;

        #endregion
    }
}
