using System.Text.Json.Serialization;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// Represents doctor profile payload returned to clients.
    /// </summary>
    public class DoctorProfile
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
        /// Gets or sets doctor specialization.
        /// </summary>
        [JsonPropertyName("Specialization")]
        public string? L03F03 { get; set; }

        /// <summary>
        /// Gets or sets doctor license number.
        /// </summary>
        [JsonPropertyName("LicenseNumber")]
        public string? L03F04 { get; set; }

        /// <summary>
        /// Gets or sets doctor years of experience.
        /// </summary>
        [JsonPropertyName("YearsExperience")]
        public int? L03F05 { get; set; }

        #endregion
    }
}
