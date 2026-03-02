using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentAPI.Exceptions
{
    /// <summary>
    /// Represents domain/business rule violations
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Construcotr of Business exception
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(string message) : base(message)
        {
        }
    }
}