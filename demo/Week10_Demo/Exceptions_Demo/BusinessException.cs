using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Exceptions_Demo
{
    /// <summary>
    /// Custom exception type to represent known business errors with a mapped HTTP status code.
    /// </summary>
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public string Detail { get; private set; }

        public BusinessException(string message, string detail, HttpStatusCode statusCode)
            : base(message)
        {
            this.Detail = detail;
            this.StatusCode = statusCode;
        }
    }
}