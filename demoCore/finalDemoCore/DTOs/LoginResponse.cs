namespace backend.DTOs
{
    /// <summary>
    /// Represents login response payload.
    /// </summary>
    public class LoginResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets operation result message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets JWT token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user profile payload.
        /// </summary>
        public object Profile { get; set; } = null!;

        #endregion
    }
}
