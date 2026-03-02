using System.Text.Json.Serialization;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// Represents patient profile payload returned to clients.
    /// </summary>
    public class PatientProfile
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets user identifier.
        /// </summary>
        [JsonPropertyName("Id")]
        public int L01F01 { get; set; }

        /// <summary>
        /// Gets or sets user role type.
        /// </summary>
        [JsonPropertyName("UserType")]
        public UserType L01F02 { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        [JsonPropertyName("FullName")]
        public string L01F03 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [JsonPropertyName("Email")]
        public string L01F04 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [JsonPropertyName("Phone")]
        public string L01F05 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        [JsonPropertyName("Dob")]
        public DateTime L01F06 { get; set; }

        /// <summary>
        /// Gets or sets profile creation date and time.
        /// </summary>
        [JsonPropertyName("CreatedAt")]
        public DateTime L01F08 { get; set; }

        /// <summary>
        /// Gets or sets emergency contact number.
        /// </summary>
        [JsonPropertyName("EmergencyContact")]
        public string? L02F03 { get; set; }

        /// <summary>
        /// Gets or sets allergy notes.
        /// </summary>
        [JsonPropertyName("Allergies")]
        public string? L02F04 { get; set; }

        #endregion
    }
}
