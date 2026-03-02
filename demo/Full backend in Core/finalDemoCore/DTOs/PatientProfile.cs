using backend.Models;
using backend.Mapping;

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
        [MapProperty("L01F01")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user role type.
        /// </summary>
        [MapProperty("L01F02")]
        public UserType UserType { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        [MapProperty("L01F03")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [MapProperty("L01F04")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [MapProperty("L01F05")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        [MapProperty("L01F06")]
        public DateTime Dob { get; set; }

        /// <summary>
        /// Gets or sets profile creation date and time.
        /// </summary>
        [MapProperty("L01F08")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets emergency contact number.
        /// </summary>
        [MapProperty("L02F03")]
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// Gets or sets allergy notes.
        /// </summary>
        [MapProperty("L02F04")]
        public string? Allergies { get; set; }

        #endregion
    }
}
