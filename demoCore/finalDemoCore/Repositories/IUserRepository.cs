using backend.Models;

namespace backend.Repositories
{
    /// <summary>
    /// Defines data access operations for user authentication and profile retrieval.
    /// </summary>
    public interface IUserRepository
    {
        #region Public Methods

        /// <summary>
        /// Creates a user record and returns its identifier.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>The created user identifier.</returns>
        Task<int> CreateUserAsync(TBL01 user);

        /// <summary>
        /// Creates a patient record.
        /// </summary>
        /// <param name="patient">The patient entity to create.</param>
        Task CreatePatientAsync(TBL02 patient);

        /// <summary>
        /// Creates a doctor record.
        /// </summary>
        /// <param name="doctor">The doctor entity to create.</param>
        Task CreateDoctorAsync(TBL03 doctor);

        /// <summary>
        /// Finds user entity by email.
        /// </summary>
        /// <param name="email">The email to search.</param>
        /// <returns>The user entity when found; otherwise null.</returns>
        Task<TBL01?> FindUserByEmailAsync(string email);

        /// <summary>
        /// Finds patient profile data by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The patient entity when found; otherwise null.</returns>
        Task<TBL02?> FindPatientByUserIdAsync(int userId);

        /// <summary>
        /// Finds doctor profile data by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The doctor entity when found; otherwise null.</returns>
        Task<TBL03?> FindDoctorByUserIdAsync(int userId);

        #endregion
    }
}
