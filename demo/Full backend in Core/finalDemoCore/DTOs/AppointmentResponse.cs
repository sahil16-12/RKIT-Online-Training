using backend.Mapping;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// Represents appointment payload returned by booking APIs.
    /// </summary>
    public class AppointmentResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets appointment identifier.
        /// </summary>
        [MapProperty("L04F01")]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Gets or sets patient user identifier.
        /// </summary>
        [MapProperty("L04F02")]
        public int PatientUserId { get; set; }

        /// <summary>
        /// Gets or sets doctor user identifier.
        /// </summary>
        [MapProperty("L04F03")]
        public int DoctorUserId { get; set; }

        /// <summary>
        /// Gets or sets appointment UTC date and time.
        /// </summary>
        [MapProperty("L04F04")]
        public DateTime AppointmentAtUtc { get; set; }

        /// <summary>
        /// Gets or sets appointment reason.
        /// </summary>
        [MapProperty("L04F05")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets appointment status.
        /// </summary>
        [MapProperty("L04F06")]
        public AppointmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets doctor notes.
        /// </summary>
        [MapProperty("L04F07")]
        public string? DoctorNotes { get; set; }

        /// <summary>
        /// Gets or sets appointment creation UTC date and time.
        /// </summary>
        [MapProperty("L04F08")]
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// Gets or sets appointment update UTC date and time.
        /// </summary>
        [MapProperty("L04F09")]
        public DateTime UpdatedAtUtc { get; set; }

        #endregion
    }
}
