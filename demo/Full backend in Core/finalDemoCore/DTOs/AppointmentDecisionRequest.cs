using backend.Models;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    /// <summary>
    /// Represents doctor action request for pending appointment.
    /// </summary>
    public class AppointmentDecisionRequest
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets decision action.
        /// </summary>
        [Required(ErrorMessage = "Decision is required.")]
        [EnumDataType(typeof(AppointmentDecisionAction), ErrorMessage = "Please select a valid decision.")]
        public AppointmentDecisionAction Decision { get; set; }

        /// <summary>
        /// Gets or sets optional doctor note.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Doctor notes cannot exceed 500 characters.")]
        public string? DoctorNotes { get; set; }

        #endregion
    }
}
