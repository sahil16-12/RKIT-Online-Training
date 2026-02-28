using backend.Models;

namespace backend.Repositories
{
    /// <summary>
    /// Defines persistence operations for appointment workflows.
    /// </summary>
    public interface IAppointmentRepository
    {
        #region Public Methods

        /// <summary>
        /// Retrieves all doctors with related user data.
        /// </summary>
        /// <returns>A list of doctor entities with user navigation loaded.</returns>
        Task<List<TBL03>> GetAllDoctorsAsync();

        /// <summary>
        /// Finds user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The matched user entity; otherwise null.</returns>
        Task<TBL01?> FindUserByIdAsync(int userId);

        /// <summary>
        /// Checks whether doctor profile exists for the given user.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <returns>True when doctor exists; otherwise false.</returns>
        Task<bool> DoesDoctorExistAsync(int doctorUserId);

        /// <summary>
        /// Checks whether doctor has pending or approved appointment at the same slot.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="appointmentAtUtc">The appointment UTC date and time.</param>
        /// <returns>True when slot is already occupied; otherwise false.</returns>
        Task<bool> IsDoctorSlotOccupiedAsync(int doctorUserId, DateTime appointmentAtUtc);

        /// <summary>
        /// Checks whether patient already has same appointment request.
        /// </summary>
        /// <param name="patientUserId">The patient user identifier.</param>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="appointmentAtUtc">The appointment UTC date and time.</param>
        /// <returns>True when duplicate appointment exists; otherwise false.</returns>
        Task<bool> IsPatientDuplicateAppointmentAsync(int patientUserId, int doctorUserId, DateTime appointmentAtUtc);

        /// <summary>
        /// Persists a new appointment and returns identifier.
        /// </summary>
        /// <param name="appointment">The appointment entity.</param>
        /// <returns>The created appointment identifier.</returns>
        Task<int> CreateAppointmentAsync(TBL04 appointment);

        /// <summary>
        /// Finds appointment by identifier.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>The appointment entity when found; otherwise null.</returns>
        Task<TBL04?> FindAppointmentByIdAsync(int appointmentId);

        /// <summary>
        /// Retrieves doctor appointments for a specific status.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="status">The status to filter.</param>
        /// <returns>A list of matching appointments.</returns>
        Task<List<TBL04>> GetDoctorAppointmentsByStatusAsync(int doctorUserId, AppointmentStatus status);

        /// <summary>
        /// Updates existing appointment entity.
        /// </summary>
        /// <param name="appointment">The appointment entity.</param>
        Task UpdateAppointmentAsync(TBL04 appointment);

        #endregion
    }
}
