namespace backend.Exceptions
{
    /// <summary>
    /// Represents an application-level exception with an HTTP status code.
    /// </summary>
    public class AppException : Exception
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public AppException(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the HTTP status code associated with this exception.
        /// </summary>
        public int StatusCode { get; }

        #endregion
    }
}
