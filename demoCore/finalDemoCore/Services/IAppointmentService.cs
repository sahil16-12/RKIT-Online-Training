using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    /// <summary>
    /// Defines business operations for appointment workflows.
    /// </summary>
    public interface IAppointmentService
    {
        #region Public Methods

        /// <summary>
        /// Retrieves available doctors for patient booking.
        /// </summary>
        /// <param name="patientUserId">The patient user identifier.</param>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="pageSize">The number of records per page (default: 10).</param>
        /// <returns>A list of available doctors.</returns>
        Task<List<AvailableDoctorResponse>> GetAvailableDoctorsAsync(int patientUserId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Validates patient role for appointment booking workflow.
        /// </summary>
        /// <param name="userType">The user type.</param>
        void PatientValidatePreSaveAsync(UserType userType);

        /// <summary>
        /// Validates doctor role for appointment booking workflow.
        /// </summary>
        /// <param name="userType">The user type.</param>
        void DoctorValidatePreSaveAsync(UserType userType);

        /// <summary>
        /// Prepares appointment creation workflow by mapping request DTO to entity.
        /// </summary>
        /// <param name="patientUserId">The patient user identifier.</param>
        /// <param name="request">The booking request payload.</param>
        /// <returns>The mapped appointment entity.</returns>
        Task<Models.TBL04> CreateAppointmentPresaveAsync(int patientUserId, CreateAppointmentRequest request);

        /// <summary>
        /// Validates prepared appointment workflow state and business rules.
        /// </summary>
        Task CreateAppointmentValidateAsync();

        /// <summary>
        /// Persists validated appointment creation workflow state.
        /// </summary>
        /// <returns>The created appointment response.</returns>
        Task<AppointmentResponse> CreateAppointmentSaveAsync();

        /// <summary>
        /// Retrieves doctor pending appointments.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <returns>A list of pending appointments.</returns>
        Task<List<AppointmentResponse>> GetPendingAppointmentsAsync(int doctorUserId);

        /// <summary>
        /// Prepares decide appointment workflow state.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="request">The decision request payload.</param>
        Task DecideAppointmentPresaveAsync(int doctorUserId, int appointmentId, AppointmentDecisionRequest request);

        /// <summary>
        /// Validates decide appointment workflow state and business rules.
        /// </summary>
        Task DecideAppointmentValidateAsync();

        /// <summary>
        /// Persists decide appointment workflow state.
        /// </summary>
        Task DecideAppointmentSaveAsync();

        /// <summary>
        /// Prepares cancel future appointment workflow state.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="request">The cancellation request payload.</param>
        Task CancelFutureAppointmentPresaveAsync(int doctorUserId, int appointmentId, CancelAppointmentRequest request);

        /// <summary>
        /// Validates cancel future appointment workflow state and business rules.
        /// </summary>
        Task CancelFutureAppointmentValidateAsync();

        /// <summary>
        /// Persists cancel future appointment workflow state.
        /// </summary>
        Task CancelFutureAppointmentSaveAsync();

        #endregion
    }
}
