using System;

namespace DoctorAppointmentAPI.Models
{
    /// <summary>
    /// Represents a user in the system (Patient or Doctor)
    /// </summary>
    public class TBL01
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int L01F01 { get; set; } // Id

        /// <summary>
        /// Username for authentication
        /// </summary>
        public string L01F02 { get; set; } // Username

        /// <summary>
        /// Hashed password
        /// </summary>
        public string L01F03 { get; set; } // Password

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string L01F04 { get; set; } // FullName

        /// <summary>
        /// Email address
        /// </summary>
        public string L01F05 { get; set; } // Email

        /// <summary>
        /// User role (Patient, Doctor, Admin)
        /// </summary>
        public int L01F06 { get; set; } // Role

        /// <summary>
        /// Specialization (for doctors only)
        /// </summary>
        public string L01F07 { get; set; } // Specialization

        /// <summary>
        /// Date when user was created
        /// </summary>
        public DateTime L01F08 { get; set; } // CreatedDate
    }
}
