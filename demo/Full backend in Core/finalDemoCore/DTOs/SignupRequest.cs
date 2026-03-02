using System.ComponentModel.DataAnnotations;
using backend.Mapping;
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
        [MapProperty("L01F02")]
        public UserType UserType { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        [Required(ErrorMessage = "Full name is required.")]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Full name can contain only letters and spaces.")]
        [MapProperty("L01F03")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [MapProperty("L01F04")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        [MapProperty("L01F05")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required.")]
        [MapProperty("L01F06")]
        public DateTime Dob { get; set; }

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
        [MapProperty("L02F03")]
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// Gets or sets patient allergy notes.
        /// </summary>
        [MapProperty("L02F04")]
        public string? Allergies { get; set; }

        /// <summary>
        /// Gets or sets doctor specialization.
        /// </summary>
        [MapProperty("L03F03")]
        public string? Specialization { get; set; }

        /// <summary>
        /// Gets or sets doctor license number.
        /// </summary>
        [MapProperty("L03F04")]
        public string? LicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets doctor years of experience.
        /// </summary>
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        [MapProperty("L03F05")]
        public int? YearsExperience { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs cross-property and role-based validation.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>Validation errors for invalid fields.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserType == UserType.DOCTOR)
            {
                if (string.IsNullOrWhiteSpace(Specialization))
                {
                    yield return new ValidationResult(
                        "Specialization is required for doctors.",
                        new[] { nameof(Specialization) });
                }

                if (string.IsNullOrWhiteSpace(LicenseNumber))
                {
                    yield return new ValidationResult(
                        "License number is required for doctors.",
                        new[] { nameof(LicenseNumber) });
                }

                if (!YearsExperience.HasValue)
                {
                    yield return new ValidationResult(
                        "Years of experience is required for doctors.",
                        new[] { nameof(YearsExperience) });
                }
            }
            else if (UserType == UserType.PATIENT)
            {
                EmergencyContact = string.IsNullOrWhiteSpace(EmergencyContact) ? null : EmergencyContact.Trim();
                Allergies = string.IsNullOrWhiteSpace(Allergies) ? null : Allergies.Trim();

                if (EmergencyContact != null && !System.Text.RegularExpressions.Regex.IsMatch(EmergencyContact, @"^[0-9]{10}$"))
                {
                    yield return new ValidationResult(
                        "Emergency contact must be exactly 10 digits when provided.",
                        new[] { nameof(EmergencyContact) });
                }

                if (Allergies != null && Allergies.Length < 3)
                {
                    yield return new ValidationResult(
                        "Allergies must be at least 3 characters when provided.",
                        new[] { nameof(Allergies) });
                }

                Specialization = null;
                LicenseNumber = null;
                YearsExperience = null;
            }
        }

        #endregion
    }
}
