using System.Text.Json.Serialization;

namespace backend.DTOs
{
    /// <summary>
    /// Represents available doctor details for appointment booking.
    /// </summary>
    public class AvailableDoctorResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets doctor user identifier.
        /// </summary>
        [JsonPropertyName("DoctorUserId")]
        public int L01F01 { get; set; }

        /// <summary>
        /// Gets or sets doctor full name.
        /// </summary>
        [JsonPropertyName("FullName")]
        public string L01F03 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets doctor email address.
        /// </summary>
        [JsonPropertyName("Email")]
        public string L01F04 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets doctor phone number.
        /// </summary>
        [JsonPropertyName("Phone")]
        public string L01F05 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets doctor specialization.
        /// </summary>
        [JsonPropertyName("Specialization")]
        public string? L03F03 { get; set; }

        /// <summary>
        /// Gets or sets doctor years of experience.
        /// </summary>
        [JsonPropertyName("YearsExperience")]
        public int? L03F05 { get; set; }

        #endregion
    }
}
