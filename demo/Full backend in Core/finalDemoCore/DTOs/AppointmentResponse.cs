using System.Text.Json.Serialization;
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
        [JsonPropertyName("AppointmentId")]
        public int L04F01 { get; set; }

        /// <summary>
        /// Gets or sets patient user identifier.
        /// </summary>
        [JsonPropertyName("PatientUserId")]
        public int L04F02 { get; set; }

        /// <summary>
        /// Gets or sets doctor user identifier.
        /// </summary>
        [JsonPropertyName("DoctorUserId")]
        public int L04F03 { get; set; }

        /// <summary>
        /// Gets or sets appointment UTC date and time.
        /// </summary>
        [JsonPropertyName("AppointmentAtUtc")]
        public DateTime L04F04 { get; set; }

        /// <summary>
        /// Gets or sets appointment reason.
        /// </summary>
        [JsonPropertyName("Reason")]
        public string L04F05 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets appointment status.
        /// </summary>
        [JsonPropertyName("Status")]
        public AppointmentStatus L04F06 { get; set; }

        /// <summary>
        /// Gets or sets doctor notes.
        /// </summary>
        [JsonPropertyName("DoctorNotes")]
        public string? L04F07 { get; set; }

        /// <summary>
        /// Gets or sets appointment creation UTC date and time.
        /// </summary>
        [JsonPropertyName("CreatedAtUtc")]
        public DateTime L04F08 { get; set; }

        /// <summary>
        /// Gets or sets appointment update UTC date and time.
        /// </summary>
        [JsonPropertyName("UpdatedAtUtc")]
        public DateTime L04F09 { get; set; }

        #endregion
    }
}
