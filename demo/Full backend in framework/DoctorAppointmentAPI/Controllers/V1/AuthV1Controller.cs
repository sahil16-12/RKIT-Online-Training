using DoctorAppointmentAPI.Data;
using DoctorAppointmentAPI.DTOs.Auth;
using DoctorAppointmentAPI.Helpers;
using DoctorAppointmentAPI.Models;
using System.Net.Mail;
using System.Web.Http;

namespace DoctorAppointmentAPI.Controllers.V1
{
    /// <summary>
    /// Version 1 Auth controller
    /// </summary>
    [RoutePrefix("api/v1/auth")]
    public class AuthV1Controller : ApiController
    {
        private readonly InMemoryDataStore _store;
        private readonly JwtHelper _jwtHelper;

        /// <summary>
        /// Version 1 Auth controller constructor
        /// </summary>
        public AuthV1Controller()
        {
            _store = InMemoryDataStore.Instance;
            _jwtHelper = new JwtHelper();
        }

        /// <summary>
        /// REGISTER
        /// Rate limited by Email (example usage)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] TBL01 user)
        {
            if (user == null)
                return BadRequest("User data is required");

            if (string.IsNullOrWhiteSpace(user.L01F02)) // Username
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(user.L01F03) || user.L01F03.Length < 6) // Password
                return BadRequest("Password must be at least 6 characters");

            if (_store.GetByUsername(user.L01F02) != null)
                return BadRequest("Username already exists");

            if (string.IsNullOrWhiteSpace(user.L01F05)) // Email
                return BadRequest("Email is required");

            if (!IsValidEmail(user.L01F05))
                return BadRequest("Invalid email format");

            // Default role if not provided
            user.L01F06 = user.L01F06 == 0 ? (int)UserRole.Patient : user.L01F06;

            TBL01 createdUser = _store.AddUser(user);

            return Ok(new
            {
                message = "User registered successfully",
                user = new
                {
                    createdUser.L01F01, // Id
                    createdUser.L01F02, // Username
                    Role = ((UserRole)createdUser.L01F06).ToString(),
                    createdUser.L01F08 // CreatedDate
                }
            });
        }

        /// <summary>
        /// Simple email validation using MailAddress class
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// LOGIN
        /// Rate limited by Username
        /// Filter will:
        /// - block when max attempts exceeded
        /// - record failed attempts when response = 401
        /// - clear attempts when login succeeds
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [RateLimit(5, 60, KeyProperty = "Username")]
        public IHttpActionResult Login([FromBody] LoginRequestDto request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required");
            }

            TBL01 user = _store.ValidateUser(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            string token = _jwtHelper.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.L01F01, // Id
                    user.L01F02, // Username
                    Role = (UserRole)user.L01F06,
                    user.L01F04 // FullName
                }
            });
        }
    }
}
