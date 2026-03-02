using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// Represents signup request payload.
    /// </summary>
    public class SignupRequest : IValidatableObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the requested user role type.
        /// </summary>
        [Required(ErrorMessage = "Please select a user type.")]
        [EnumDataType(typeof(UserType), ErrorMessage = "Please select a valid user type.")]
        [JsonPropertyName("UserType")]
        public UserType L01F02 { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        [Required(ErrorMessage = "Full name is required.")]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Full name can contain only letters and spaces.")]
        [JsonPropertyName("FullName")]
        public string L01F03 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [JsonPropertyName("Email")]
        public string L01F04 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        [JsonPropertyName("Phone")]
        public string L01F05 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required.")]
        [JsonPropertyName("Dob")]
        public DateTime L01F06 { get; set; }

        /// <summary>
        /// Gets or sets password text.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets patient emergency contact number.
        /// </summary>
        [RegularExpression(@"^$|^[0-9]{10}$", ErrorMessage = "Emergency contact must be exactly 10 digits.")]
        [JsonPropertyName("EmergencyContact")]
        public string? L02F03 { get; set; }

        /// <summary>
        /// Gets or sets patient allergy notes.
        /// </summary>
        [JsonPropertyName("Allergies")]
        public string? L02F04 { get; set; }

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
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        [JsonPropertyName("YearsExperience")]
        public int? L03F05 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs cross-property and role-based validation.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>Validation errors for invalid fields.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (L01F02 == UserType.DOCTOR)
            {
                if (string.IsNullOrWhiteSpace(L03F03))
                {
                    yield return new ValidationResult(
                        "Specialization is required for doctors.",
                        new[] { nameof(L03F03) });
                }

                if (string.IsNullOrWhiteSpace(L03F04))
                {
                    yield return new ValidationResult(
                        "License number is required for doctors.",
                        new[] { nameof(L03F04) });
                }

                if (!L03F05.HasValue)
                {
                    yield return new ValidationResult(
                        "Years of experience is required for doctors.",
                        new[] { nameof(L03F05) });
                }
            }
            else if (L01F02 == UserType.PATIENT)
            {
                L02F03 = string.IsNullOrWhiteSpace(L02F03) ? null : L02F03.Trim();
                L02F04 = string.IsNullOrWhiteSpace(L02F04) ? null : L02F04.Trim();

                if (L02F03 != null && !System.Text.RegularExpressions.Regex.IsMatch(L02F03, @"^[0-9]{10}$"))
                {
                    yield return new ValidationResult(
                        "Emergency contact must be exactly 10 digits when provided.",
                        new[] { nameof(L02F03) });
                }

                if (L02F04 != null && L02F04.Length < 3)
                {
                    yield return new ValidationResult(
                        "Allergies must be at least 3 characters when provided.",
                        new[] { nameof(L02F04) });
                }

                L03F03 = null;
                L03F04 = null;
                L03F05 = null;
            }
        }

        #endregion
    }
}
