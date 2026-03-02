using System.ComponentModel.DataAnnotations;
using backend.Mapping;

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
        [MapProperty("L04F07")]
        public string DoctorNotes { get; set; } = string.Empty;

        #endregion
    }
}
