using DoctorAppointmentAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentAPI.DTOs.Auth
{
    /// <summary>
    /// DTO representing the registration request
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// Representing the username of user
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Representing the password of user
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// Representing the fullname of user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Representing the email of user
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Representing the role of user
        /// </summary>
        public UserRole? Role { get; set; }
    }
}
