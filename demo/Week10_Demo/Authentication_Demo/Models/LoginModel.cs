namespace Authentication_Demo.Models
{
    /// <summary>
    /// Represents the data required for user login.
    /// This model is used to receive credentials from the client
    /// during authentication requests.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; }
    }
}
