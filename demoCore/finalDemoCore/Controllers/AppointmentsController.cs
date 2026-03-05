using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Extensions;
using backend.Exceptions;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    /// <summary>
    /// Exposes appointment booking endpoints for patients and doctors.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        #region Private Fields

        /// <summary>
        /// Represents appointment service dependency.
        /// </summary>
        private readonly IAppointmentService _appointmentService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentsController"/> class.
        /// </summary>
        /// <param name="appointmentService">The appointment service.</param>
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        #endregion

        #region Patient Endpoints

        /// <summary>
        /// Retrieves available doctors for appointment booking.
        /// </summary>
        /// <returns>A list of available doctors.</returns>
        [HttpGet("doctors/available")]
        public async Task<IActionResult> GetAvailableDoctors()
        {
            CurrentUserContext currentUser = HttpContext.GetCurrentUserContext();

            if (currentUser.Role != UserType.PATIENT)
            {
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }

            List<AvailableDoctorResponse> response = await _appointmentService.GetAvailableDoctorsAsync(currentUser.UserId);

            return Ok(response);
        }

        /// <summary>
        /// Creates a new appointment request by patient.
        /// </summary>
        /// <param name="request">The booking request payload.</param>
        /// <returns>The created appointment details.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            CurrentUserContext currentUser = HttpContext.GetCurrentUserContext();

            ////HAVE TO ADD TTHIS IN VALIDATEEFOREPRESAVE
            if (currentUser.Role != UserType.PATIENT)
            {
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }

            await _appointmentService.CreateAppointmentPresaveAsync(currentUser.UserId, request);
            await _appointmentService.CreateAppointmentValidateAsync();
            AppointmentResponse response = await _appointmentService.CreateAppointmentSaveAsync();

            return StatusCode(201, response);
        }

        #endregion

        #region Doctor Endpoints

        /// <summary>
        /// Retrieves doctor pending appointment requests.
        /// </summary>
        /// <returns>A list of pending appointments.</returns>
        [HttpGet("doctor/pending")]
        public async Task<IActionResult> GetPendingAppointments()
        {
            CurrentUserContext currentUser = HttpContext.GetCurrentUserContext();

            if (currentUser.Role != UserType.DOCTOR)
            {
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }

            List<AppointmentResponse> response = await _appointmentService.GetPendingAppointmentsAsync(currentUser.UserId);

            return Ok(response);
        }

        /// <summary>
        /// Approves or declines a pending appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="request">The decision request payload.</param>
        /// <returns>A status response for decision action.</returns>
        [HttpPut("{appointmentId:int}/decision")]
        public async Task<IActionResult> DecideAppointment(int appointmentId, [FromBody] AppointmentDecisionRequest request)
        {
            CurrentUserContext currentUser = HttpContext.GetCurrentUserContext();

            if (currentUser.Role != UserType.DOCTOR)
            {
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }

            await _appointmentService.DecideAppointmentPresaveAsync(currentUser.UserId, appointmentId, request);
            await _appointmentService.DecideAppointmentValidateAsync();
            await _appointmentService.DecideAppointmentSaveAsync();

            return Ok(new { message = "Appointment updated successfully." });
        }

        /// <summary>
        /// Cancels a future appointment by doctor.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="request">The cancellation request payload.</param>
        /// <returns>A status response for cancellation action.</returns>
        [HttpPut("{appointmentId:int}/cancel")]
        public async Task<IActionResult> CancelFutureAppointment(int appointmentId, [FromBody] CancelAppointmentRequest request)
        {
            CurrentUserContext currentUser = HttpContext.GetCurrentUserContext();

            if (currentUser.Role != UserType.DOCTOR)
            {
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }

            await _appointmentService.CancelFutureAppointmentPresaveAsync(currentUser.UserId, appointmentId, request);
            await _appointmentService.CancelFutureAppointmentValidateAsync();
            await _appointmentService.CancelFutureAppointmentSaveAsync();

            return Ok(new { message = "Appointment cancelled successfully." });
        }

        #endregion

    }
}
