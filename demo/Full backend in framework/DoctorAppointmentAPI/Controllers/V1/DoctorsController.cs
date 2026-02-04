using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using DoctorAppointmentAPI.Data;
using DoctorAppointmentAPI.Models;

namespace DoctorAppointmentAPI.Controllers.V1
{
    /// <summary>
    /// Controller for doctor-related operations
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly InMemoryDataStore _store;

        /// <summary>
        /// Version 1 Doctor controller
        /// </summary>
        public DoctorsController()
        {
            _store = InMemoryDataStore.Instance;
        }

        // Helpers

        private UserRole CurrentRole
        {
            get
            {
                Claim roleClaim = ((ClaimsPrincipal)User).FindFirst(ClaimTypes.Role);
                return roleClaim == null
                    ? UserRole.Patient
                    : (UserRole)Enum.Parse(typeof(UserRole), roleClaim.Value);
            }
        }

        /// <summary>
        /// GET: All doctors (Any authenticated user)
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllDoctors()
        {
            List<TBL01> doctors = _store
                .GetAllUsers()
                .Where(u => (UserRole)u.L01F06 == UserRole.Doctor) // Role
                .Select(d => new TBL01
                {
                    L01F01 = d.L01F01, // Id
                    L01F02 = d.L01F02, // Username
                    L01F04 = d.L01F04, // FullName
                    L01F05 = d.L01F05, // Email
                    L01F06 = d.L01F06, // Role
                    L01F07 = d.L01F07, // Specialization
                    L01F08 = d.L01F08  // CreatedDate
                })
                .ToList();

            return Ok(doctors);
        }

        /// <summary>
        /// GET: Doctor by ID
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetDoctorById(int id)
        {
            TBL01 doctor = _store.GetUserById(id);

            if (doctor == null || (UserRole)doctor.L01F06 != UserRole.Doctor)
                return NotFound();

            // Remove sensitive data
            doctor.L01F03 = null; // Password

            return Ok(doctor);
        }

        /// <summary>
        /// GET : Doctors by specialization
        /// </summary>
        [HttpGet]
        [Route("specialization/{specialization}")]
        public IHttpActionResult GetDoctorsBySpecialization(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return BadRequest("Specialization is required");

            List<TBL01> doctors = _store
                .GetAllUsers()
                .Where(u =>
                    (UserRole)u.L01F06 == UserRole.Doctor &&
                    !string.IsNullOrWhiteSpace(u.L01F07) &&
                    u.L01F07.Equals(specialization, StringComparison.OrdinalIgnoreCase))
                .Select(d => new TBL01
                {
                    L01F01 = d.L01F01, // Id
                    L01F02 = d.L01F02, // Username
                    L01F04 = d.L01F04, // FullName
                    L01F05 = d.L01F05, // Email
                    L01F06 = d.L01F06, // Role
                    L01F07 = d.L01F07  // Specialization
                })
                .ToList();

            return Ok(doctors);
        }
    }
}
