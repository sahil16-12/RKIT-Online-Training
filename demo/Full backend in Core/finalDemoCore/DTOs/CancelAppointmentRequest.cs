using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.DTOs
{
    /// <summary>
    /// Represents doctor request for cancelling a future appointment.
    /// </summary>
    public class CancelAppointmentRequest
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets cancellation note.
        /// </summary>
        [Required(ErrorMessage = "Cancellation note is required.")]
        [MinLength(3, ErrorMessage = "Cancellation note must be at least 3 characters.")]
        [MaxLength(500, ErrorMessage = "Cancellation note cannot exceed 500 characters.")]
        [JsonPropertyName("DoctorNotes")]
        public string L04F07 { get; set; } = string.Empty;

        #endregion
    }
}
