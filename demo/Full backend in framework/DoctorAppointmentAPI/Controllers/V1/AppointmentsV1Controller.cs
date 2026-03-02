using DoctorAppointmentAPI.Data;
using DoctorAppointmentAPI.Exceptions;
using DoctorAppointmentAPI.Models;
using System;
using System.Security.Claims;
using System.Web.Http;

namespace DoctorAppointmentAPI.Controllers.V1
{
    /// <summary>
    /// Version 1 appointments controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/appointments")]
    
    public class AppointmentsV1Controller : ApiController
    {
        private readonly InMemoryDataStore _store;

        /// <summary>
        /// constructor of v1 appointments controller
        /// </summary>
        public AppointmentsV1Controller()
        {
            _store = InMemoryDataStore.Instance;
        }

        // Helpers
        private int UserId =>
            int.Parse(((ClaimsPrincipal)User).FindFirst("userId").Value);

        private UserRole Role =>
            (UserRole)Enum.Parse(typeof(UserRole),
                ((ClaimsPrincipal)User).FindFirst(ClaimTypes.Role).Value);
        /// <summary>
        /// BOOK APPOINTMENT (Patient)
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        [Route("book")]
        public IHttpActionResult Book(TBL03 appointment)
        {
            if (Role != UserRole.Patient)
                throw new BusinessException("Only patients can book appointments");

            if (appointment.L03F04 <= DateTime.UtcNow)
                throw new BusinessException("Appointment must be in the future");

            appointment.L03F03 = UserId;

            TBL03 created = _store.BookAppointment(appointment);
            return Ok(created);
        }

        /// <summary>
        /// MY APPOINTMENTS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        public IHttpActionResult MyAppointments()
        {
            if (Role == UserRole.Patient)
                return Ok(_store.GetByPatient(UserId));

            if (Role == UserRole.Doctor)
                return Ok(_store.GetByDoctor(UserId));

            return Ok(_store.GetAllAppointments());
        }

        /// <summary>
        /// UPDATE STATUS (Doctor)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPut]
        [Route("{id:int}/status")]
        public IHttpActionResult UpdateStatus(int id, [FromBody] string status)
        {
            if (Role != UserRole.Doctor)
                throw new BusinessException("Only doctors can update status");

            _store.UpdateStatus(id, status);
            return Ok("Status updated");
        }

        /// <summary>
        /// CANCEL APPOINTMENT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Cancel(int id)
        {
            _store.Cancel(id);
            return Ok("Appointment cancelled");
        }
    }
}
