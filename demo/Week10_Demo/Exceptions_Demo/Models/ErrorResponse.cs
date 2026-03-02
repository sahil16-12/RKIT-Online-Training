using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exceptions_Demo.Models
{
    /// <summary>
    /// Standardized error response payload shape returned to clients.
    /// </summary>
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Detail { get; set; }
    }
}