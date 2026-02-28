using backend.DTOs;
using backend.Exceptions;
using backend.Mapping;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    /// <summary>
    /// Provides authentication and signup business logic.
    /// </summary>
    public class AuthService : IAuthService
    {
        #region Private Fields

        /// <summary>
        /// Represents the user repository dependency.
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Represents the token service dependency.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Represents the reflection mapper dependency.
        /// </summary>
        private readonly IReflectionMapper _reflectionMapper;

        /// <summary>
        /// Represents in-memory workflow state for signup request.
        /// </summary>
        private SignupWorkflowState? _signupState;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="tokenService">The token service.</param>
        /// <param name="reflectionMapper">The reflection mapper.</param>
        public AuthService(IUserRepository userRepository, ITokenService tokenService, IReflectionMapper reflectionMapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _reflectionMapper = reflectionMapper;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public Task<TBL01> PreSaveAsync(SignupRequest request)
        {
            TBL01 user = _reflectionMapper.Map<SignupRequest, TBL01>(request);
            user.L01F07 = BCrypt.Net.BCrypt.HashPassword(request.Password);

            TBL02? patient = null;
            TBL03? doctor = null;
           
            if (request.UserType == UserType.PATIENT)
            {
                patient = _reflectionMapper.Map<SignupRequest, TBL02>(request);
            }
            else if (request.UserType == UserType.DOCTOR)
            {
                doctor = _reflectionMapper.Map<SignupRequest, TBL03>(request);
            }

            _signupState = new SignupWorkflowState
            {
                Request = request,
                User = user,
                Patient = patient,
                Doctor = doctor
            };

            return Task.FromResult(user);
        }

        /// <inheritdoc/>
        public async Task ValidateAsync()
        {
            if (_signupState == null)
            {
                throw new AppException("Invalid signup workflow state.", StatusCodes.Status400BadRequest);
            }

            TBL01? existingUser = await _userRepository.FindUserByEmailAsync(_signupState.Request.Email);
            if (existingUser != null)
            {
                throw new AppException("Email already registered.", StatusCodes.Status409Conflict);
            }
        }

        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            if (_signupState == null)
            {
                throw new AppException("Invalid signup workflow state.", StatusCodes.Status400BadRequest);
            }

            int userId = await _userRepository.CreateUserAsync(_signupState.User);

            if (_signupState.Request.UserType == UserType.PATIENT && _signupState.Patient != null)
            {
                _signupState.Patient.L02F02 = userId;
                await _userRepository.CreatePatientAsync(_signupState.Patient);
            }
            else if (_signupState.Request.UserType == UserType.DOCTOR && _signupState.Doctor != null)
            {
                _signupState.Doctor.L03F02 = userId;
                await _userRepository.CreateDoctorAsync(_signupState.Doctor);
            }

            _signupState = null;
        }

        /// <inheritdoc/>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            TBL01? user = await _userRepository.FindUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new AppException("Invalid credentials.", StatusCodes.Status401Unauthorized);
            }

            if (user.L01F02 != request.UserType)
            {
                throw new AppException(
                    $"You are registered as {user.L01F02}. Please login as {user.L01F02}.",
                    StatusCodes.Status401Unauthorized);
            }

            bool isMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.L01F07);
            if (!isMatch)
            {
                throw new AppException("Invalid credentials.", StatusCodes.Status401Unauthorized);
            }

            string token = _tokenService.GenerateToken(user.L01F01, user.L01F02);

            object profile;
            if (user.L01F02 == UserType.PATIENT)
            {
                PatientProfile patientProfile = _reflectionMapper.Map<TBL01, PatientProfile>(user);
                TBL02? patient = await _userRepository.FindPatientByUserIdAsync(user.L01F01);
                if (patient != null)
                {
                    _reflectionMapper.MapToExisting<TBL02, PatientProfile>(patient, patientProfile);
                }

                profile = patientProfile;
            }
            else
            {
                DoctorProfile doctorProfile = _reflectionMapper.Map<TBL01, DoctorProfile>(user);
                TBL03? doctor = await _userRepository.FindDoctorByUserIdAsync(user.L01F01);
                if (doctor != null)
                {
                    _reflectionMapper.MapToExisting<TBL03, DoctorProfile>(doctor, doctorProfile);
                }

                profile = doctorProfile;
            }

            LoginResponse response = new LoginResponse
            {
                Message = "Login successful.",
                Token = token,
                Profile = profile
            };

            return response;
        }

        #endregion
    }
}
