using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    /// <summary>
    /// Represents login request payload.
    /// </summary>
    public class LoginRequest
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets user email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user password text.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user role used for role-specific login.
        /// </summary>
        [Required(ErrorMessage = "Please select a user type.")]
        [EnumDataType(typeof(UserType), ErrorMessage = "Please select a valid user type.")]
        public UserType UserType { get; set; }

        #endregion
    }
}
