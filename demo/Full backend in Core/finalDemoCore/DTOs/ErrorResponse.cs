namespace backend.DTOs
{
    /// <summary>
    /// Represents API error payload.
    /// </summary>
    public class ErrorResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets error message text.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        #endregion
    }
}
