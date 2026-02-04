using DoctorAppointmentAPI.Models;

namespace DoctorAppointmentAPI.DTOs.Auth
{
    /// <summary>
    /// DTO representing response of login 
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT token of user
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// UserID of user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Represents role of user
        /// </summary>
        public UserRole Role { get; set; }
    }
}
