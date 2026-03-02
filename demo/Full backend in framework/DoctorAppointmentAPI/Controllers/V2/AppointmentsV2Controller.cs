using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using DoctorAppointmentAPI.Data;
using DoctorAppointmentAPI.DTOs.Appointments;
using DoctorAppointmentAPI.Helpers;
using DoctorAppointmentAPI.Mappers;
using DoctorAppointmentAPI.Models;

namespace DoctorAppointmentAPI.Controllers.V2
{
    /// <summary>
    /// Appointments v2 controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v2/appointments")]
    public class AppointmentsV2Controller : ApiController
    {
        private readonly InMemoryDataStore _store;

        /// <summary>
        /// Constructor of v2 appointments controller
        /// </summary>
        public AppointmentsV2Controller()
        {
            _store = InMemoryDataStore.Instance;
        }

        private int CurrentUserId =>
            int.Parse(((ClaimsPrincipal)User).FindFirst("userId").Value);

        private UserRole Role =>
            (UserRole)Enum.Parse(typeof(UserRole),
                ((ClaimsPrincipal)User).FindFirst(ClaimTypes.Role).Value);

        /// <summary>
        /// Book appointment v2
        /// </summary>
        [HttpPost]
        [Route("book")]
        [RateLimit(2, 60, KeyFromClaim = "userId")]
        public IHttpActionResult Book(CreateAppointmentRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.AppointmentDate <= DateTime.UtcNow)
                return BadRequest("Date must be in future");

            TBL03 appointment = MappingProfile.Mapper.Map<TBL03>(dto);
            appointment.L03F03 = CurrentUserId; // PatientId
            appointment.L03F05 = "Pending";

            TBL03 created = _store.BookAppointment(appointment);

            AppointmentResponseDto response =
                MappingProfile.Mapper.Map<AppointmentResponseDto>(created);

            return Ok(response);
        }


        /// <summary>
        /// Get all appointments v2
        /// </summary>
        [HttpGet]
        [Route("my")]
        public IHttpActionResult MyAppointments()
        {
            List<TBL03> li = new List<TBL03>();

            if (Role == UserRole.Patient)
                li = _store.GetByPatient(CurrentUserId);

            if (Role == UserRole.Doctor)
                li = _store.GetByDoctor(CurrentUserId);

            var list = li
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.L03F01,
                    DoctorId = a.L03F02,
                    PatientId = a.L03F03,
                    AppointmentDate = a.L03F04,
                    Status = a.L03F05
                })
                .ToList();

            return Ok(list);
        }
    }
}
