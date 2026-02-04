using System;

namespace DoctorAppointmentAPI.Models
{
    /// <summary>
    /// Appointment model
    /// </summary>
    public class TBL03
    {
        /// <summary>
        /// Represents appointment ID
        /// </summary>
        public int L03F01 { get; set; } // Id

        /// <summary>
        /// Represents doctor ID
        /// </summary>
        public int L03F02 { get; set; } // DoctorId

        /// <summary>
        /// Represents patient ID
        /// </summary>
        public int L03F03 { get; set; } // PatientId

        /// <summary>
        /// Represents appointment date
        /// </summary>
        public DateTime L03F04 { get; set; } // AppointmentDate

        /// <summary>
        /// Represents status of appointment
        /// </summary>
        public string L03F05 { get; set; } // Status

        /// <summary>
        /// Representing the date when appointment was created
        /// </summary>
        public DateTime L03F06 { get; set; } // CreatedDate
    }
}
