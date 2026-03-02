using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentAPI.DTOs.Auth
{
    /// <summary>
    /// Login request DTO
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Represents username of user
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Represents password of user
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
