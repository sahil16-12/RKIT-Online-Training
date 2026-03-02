namespace backend.Models
{
    /// <summary>
    /// Represents authenticated user context extracted from JWT claims.
    /// </summary>
    public class CurrentUserContext
    {
        #region Public Constants

        public const string ItemKey = "CurrentUserContext";

        #endregion

        #region Public Properties

        public int UserId { get; set; }

        public UserType Role { get; set; }

        #endregion
    }
}
