using backend.Models;

namespace backend.Services
{
    /// <summary>
    /// Defines JWT token generation operations.
    /// </summary>
    public interface ITokenService
    {
        #region Public Methods

        /// <summary>
        /// Generates a JWT token for a user identity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userType">The user role.</param>
        /// <returns>A signed JWT token.</returns>
        string GenerateToken(int userId, UserType userType);

        #endregion
    }
}
