using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    /// <summary>
    /// Defines authentication and registration operations.
    /// </summary>
    public interface IAuthService
    {
        #region Public Methods

        /// <summary>
        /// Performs pre-save mapping for signup and stores workflow state.
        /// </summary>
        /// <param name="request">The signup request data.</param>
        /// <returns>The mapped user POCO.</returns>
        Task<TBL01> PreSaveAsync(SignupRequest request);

        /// <summary>
        /// Performs signup validations that require data-store access.
        /// </summary>
        Task ValidateAsync();

        /// <summary>
        /// Persists signup workflow state to data-store.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Authenticates an existing user.
        /// </summary>
        /// <param name="request">The login request data.</param>
        /// <returns>The login response.</returns>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        #endregion
    }
}
