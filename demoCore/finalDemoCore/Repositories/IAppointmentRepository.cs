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

        #endregion
    }
}
