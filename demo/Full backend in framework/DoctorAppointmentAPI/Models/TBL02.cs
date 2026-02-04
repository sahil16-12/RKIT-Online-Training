using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentAPI.Models
{
    /// <summary>
    /// Represents the attempt
    /// </summary>
    public class TBL02 // Attempt 
    {
        /// <summary>
        /// Represents the number of times, login was failed
        /// </summary>
        public int L02F01; // Count 

        /// <summary>
        /// Represetns the time of first failed attempt
        /// </summary>
        public DateTime L02F02; // FirstAttemptTime 
    }
}
