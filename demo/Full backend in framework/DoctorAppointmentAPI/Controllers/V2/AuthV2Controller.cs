using DoctorAppointmentAPI.Data;
using DoctorAppointmentAPI.DTOs.Auth;
using DoctorAppointmentAPI.Helpers;
using DoctorAppointmentAPI.Mappers;
using DoctorAppointmentAPI.Models;
using System.Web.Http;

namespace DoctorAppointmentAPI.Controllers.V2
{
    /// <summary>
    /// Version 2 auth controller
    /// </summary>
    [RoutePrefix("api/v2/auth")]
    public class AuthV2Controller : ApiController
    {
        private readonly InMemoryDataStore _store;
        private readonly JwtHelper _jwtHelper;

        /// <summary>
        /// Constructor of v2 auth controller
        /// </summary>
        public AuthV2Controller()
        {
            _store = InMemoryDataStore.Instance;
            _jwtHelper = new JwtHelper();
        }

        /// <summary>
        /// Registration of v2 auth controller
        /// </summary>
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_store.GetByUsername(dto.Username) != null)
                return BadRequest("Username already exists");

            TBL01 user = MappingProfile.Mapper.Map<TBL01>(dto);

            TBL01 created = _store.AddUser(user);

            return Ok(new
            {
                message = "Registered successfully",
                userId = created.L01F01
            });
        }


        /// <summary>
        /// Login of v2 auth controller
        /// </summary>
        [HttpPost]
        [Route("login")]
        [RateLimit(windowSeconds: 30, KeyProperty = "Username")]
        public IHttpActionResult Login(LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TBL01 user = _store.ValidateUser(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized();

            string token = _jwtHelper.GenerateToken(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                UserId = user.L01F01,
                Username = user.L01F02,
                Role = (UserRole)user.L01F06
            });
        }
    }
}
