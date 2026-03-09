using backend.DTOs;
using backend.Exceptions;
using backend.Mapping;
using backend.Models;
using backend.Repositories;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace backend.Services
{
    /// <summary>
    /// Provides business logic for appointment booking operations.
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        #region Private Fields

        /// <summary>
        /// Represents appointment repository dependency.
        /// </summary>
        private readonly IAppointmentRepository _appointmentRepository;


        /// <summary>
        /// Represents database connection factory.
        /// </summary>
        private readonly IDbConnectionFactory _dbFactory;

        /// <summary>
        /// Represents reflection mapper dependency.
        /// </summary>
        private readonly IReflectionMapper _reflectionMapper;

        /// <summary>
        /// Represents in-memory workflow state for appointment creation.
        /// </summary>
        private AppointmentCreateWorkflowState? _createAppointmentState;

        /// <summary>
        /// Represents in-memory workflow state for appointment decision.
        /// </summary>
        private AppointmentDecisionWorkflowState? _decideAppointmentState;

        /// <summary>
        /// Represents in-memory workflow state for appointment cancellation.
        /// </summary>
        private AppointmentCancelWorkflowState? _cancelAppointmentState;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentService"/> class.
        /// </summary>
        /// <param name="appointmentRepository">The appointment repository.</param>
        /// <param name="reflectionMapper">The reflection mapper.</param>
        public AppointmentService(IAppointmentRepository appointmentRepository, IReflectionMapper reflectionMapper, IDbConnectionFactory dbfactory)
        {
            _appointmentRepository = appointmentRepository;
            _reflectionMapper = reflectionMapper;
            _dbFactory = dbfactory;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<List<AvailableDoctorResponse>> GetAvailableDoctorsAsync(int patientUserId, int page = 1, int pageSize = 10)
        {
            TBL01? patient = await FindUserByIdAsync(patientUserId);
            if (patient == null || patient.L01F02 != UserType.PATIENT)
            {
                throw new AppException("Patient profile not found.", StatusCodes.Status400BadRequest);
            }

            List<TBL03> doctors = await GetAllDoctorsAsync(page, pageSize);
            List<AvailableDoctorResponse> doctorResponses = new List<AvailableDoctorResponse>();

            foreach (TBL03 doctor in doctors)
            {
                if (doctor.L03F07 == null || doctor.L03F07.L01F02 != UserType.DOCTOR)
                {
                    continue;
                }

                AvailableDoctorResponse response = _reflectionMapper.Map<TBL01, AvailableDoctorResponse>(doctor.L03F07);
                _reflectionMapper.MapToExisting<TBL03, AvailableDoctorResponse>(doctor, response);
                doctorResponses.Add(response);
            }

            return doctorResponses;
        }
        /// <inheritdoc/>
        public void PatientValidatePreSaveAsync(UserType userType){
            if(userType != UserType.PATIENT){
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }
        }
        /// <inheritdoc/>
        public void DoctorValidatePreSaveAsync(UserType userType){
            if(userType != UserType.DOCTOR){
                throw new AppException("You are not authorized to access this resource.", StatusCodes.Status403Forbidden);
            }
        }

        /// <inheritdoc/>
        public Task<TBL04> CreateAppointmentPresaveAsync(int patientUserId, CreateAppointmentRequest request)
        {
            TBL04 appointment = _reflectionMapper.Map<CreateAppointmentRequest, TBL04>(request);
            DateTime normalizedAppointmentUtc = DateTime.SpecifyKind(request.L04F04, DateTimeKind.Utc);

            appointment.L04F02 = patientUserId;
            appointment.L04F04 = normalizedAppointmentUtc;
            appointment.L04F06 = AppointmentStatus.PENDING;
            appointment.L04F08 = DateTime.UtcNow;
            appointment.L04F09 = DateTime.UtcNow;

            _createAppointmentState = new AppointmentCreateWorkflowState
            {
                PatientUserId = patientUserId,
                Request = request,
                Appointment = appointment
            };

            return Task.FromResult(appointment);
        }

        /// <inheritdoc/>
        public async Task CreateAppointmentValidateAsync()
        {
            if (_createAppointmentState == null)
            {
                throw new AppException("Invalid appointment workflow state.", StatusCodes.Status400BadRequest);
            }

            TBL01? patient = await FindUserByIdAsync(_createAppointmentState.PatientUserId);
            if (patient == null || patient.L01F02 != UserType.PATIENT)
            {
                throw new AppException("Patient profile not found.", StatusCodes.Status400BadRequest);
            }

            bool doctorExists = await DoesDoctorExistAsync(_createAppointmentState.Request.L04F03);
            if (!doctorExists)
            {
                throw new AppException("Doctor not found.", StatusCodes.Status404NotFound);
            }

            DateTime normalizedAppointmentUtc = _createAppointmentState.Appointment.L04F04;
            if (normalizedAppointmentUtc <= DateTime.UtcNow)
            {
                throw new AppException("Appointment time must be in the future.", StatusCodes.Status400BadRequest);
            }

            bool isDoctorSlotOccupied = await _appointmentRepository
                .IsDoctorSlotOccupiedAsync(_createAppointmentState.Request.L04F03, normalizedAppointmentUtc);
            if (isDoctorSlotOccupied)
            {
                throw new AppException("Selected doctor slot is not available.", StatusCodes.Status400BadRequest);
            }

            bool hasDuplicateAppointment = await _appointmentRepository
                .IsPatientDuplicateAppointmentAsync(
                    _createAppointmentState.PatientUserId,
                    _createAppointmentState.Request.L04F03,
                    normalizedAppointmentUtc);
            if (hasDuplicateAppointment)
            {
                throw new AppException("Duplicate appointment request already exists.", StatusCodes.Status400BadRequest);
            }
        }

        /// <inheritdoc/>
        public async Task<AppointmentResponse> CreateAppointmentSaveAsync()
        {
            _createAppointmentState.Appointment.L04F08 = DateTime.UtcNow;
            _createAppointmentState.Appointment.L04F09 = DateTime.UtcNow;

            int appointmentId = await CreateAppointmentAsync(_createAppointmentState.Appointment);

            _createAppointmentState.Appointment.L04F01 = appointmentId;

            AppointmentResponse response = _reflectionMapper.Map<TBL04, AppointmentResponse>(_createAppointmentState.Appointment);
            _createAppointmentState = null;

            return response;
        }

        /// <inheritdoc/>
        public async Task<List<AppointmentResponse>> GetPendingAppointmentsAsync(int doctorUserId)
        {
            bool doctorExists = await DoesDoctorExistAsync(doctorUserId);
            if (!doctorExists)
            {
                throw new AppException("Doctor profile not found.", StatusCodes.Status400BadRequest);
            }

            List<TBL04> appointments = await GetDoctorAppointmentsByStatusAsync(doctorUserId, AppointmentStatus.PENDING);

            List<AppointmentResponse> responses = appointments
                .Select(appointment => _reflectionMapper.Map<TBL04, AppointmentResponse>(appointment))
                .ToList();

            return responses;
        }

        /// <inheritdoc/>
        public Task DecideAppointmentPresaveAsync(int doctorUserId, int appointmentId, AppointmentDecisionRequest request)
        {
            _decideAppointmentState = new AppointmentDecisionWorkflowState
            {
                DoctorUserId = doctorUserId,
                AppointmentId = appointmentId,
                Request = request
            };

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task DecideAppointmentValidateAsync()
        {
            if (_decideAppointmentState == null)
            {
                throw new AppException("Invalid appointment workflow state.", StatusCodes.Status400BadRequest);
            }

            TBL04 appointment = await ValidateDoctorAndGetOwnedAppointmentAsync(
                _decideAppointmentState.DoctorUserId,
                _decideAppointmentState.AppointmentId);

            if (appointment.L04F06 != AppointmentStatus.PENDING)
            {
                throw new AppException("Only pending appointments can be approved or declined.", StatusCodes.Status400BadRequest);
            }

            _decideAppointmentState.Appointment = appointment;
        }

        /// <inheritdoc/>
        public async Task DecideAppointmentSaveAsync()
        {
            if (_decideAppointmentState?.Appointment == null)
            {
                throw new AppException("Invalid appointment workflow state.", StatusCodes.Status400BadRequest);
            }

            _decideAppointmentState.Appointment.L04F06 = _decideAppointmentState.Request.Decision == AppointmentDecisionAction.APPROVE
                ? AppointmentStatus.APPROVED
                : AppointmentStatus.DECLINED;
            _decideAppointmentState.Appointment.L04F07 = _decideAppointmentState.Request.L04F07;
            _decideAppointmentState.Appointment.L04F09 = DateTime.UtcNow;

            await UpdateAppointmentAsync(_decideAppointmentState.Appointment);
            _decideAppointmentState = null;
        }

        /// <inheritdoc/>
        public Task CancelFutureAppointmentPresaveAsync(int doctorUserId, int appointmentId, CancelAppointmentRequest request)
        {
            _cancelAppointmentState = new AppointmentCancelWorkflowState
            {
                DoctorUserId = doctorUserId,
                AppointmentId = appointmentId,
                Request = request
            };

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task CancelFutureAppointmentValidateAsync()
        {
            if (_cancelAppointmentState == null)
            {
                throw new AppException("Invalid appointment workflow state.", StatusCodes.Status400BadRequest);
            }

            TBL04 appointment = await ValidateDoctorAndGetOwnedAppointmentAsync(
                _cancelAppointmentState.DoctorUserId,
                _cancelAppointmentState.AppointmentId);

            if (appointment.L04F04 <= DateTime.UtcNow)
            {
                throw new AppException("Only future appointments can be cancelled.", StatusCodes.Status400BadRequest);
            }

            if (appointment.L04F06 != AppointmentStatus.PENDING && appointment.L04F06 != AppointmentStatus.APPROVED)
            {
                throw new AppException("Only pending or approved appointments can be cancelled.", StatusCodes.Status400BadRequest);
            }

            _cancelAppointmentState.Appointment = appointment;
        }

        /// <inheritdoc/>
        public async Task CancelFutureAppointmentSaveAsync()
        {
            if (_cancelAppointmentState?.Appointment == null)
            {
                throw new AppException("Invalid appointment workflow state.", StatusCodes.Status400BadRequest);
            }

            _reflectionMapper.MapToExisting<CancelAppointmentRequest, TBL04>(
                _cancelAppointmentState.Request,
                _cancelAppointmentState.Appointment);
            _cancelAppointmentState.Appointment.L04F06 = AppointmentStatus.CANCELLED;
            _cancelAppointmentState.Appointment.L04F09 = DateTime.UtcNow;

            await UpdateAppointmentAsync(_cancelAppointmentState.Appointment);
            _cancelAppointmentState = null;
        }

        /// <summary>
        /// Retrieves all doctors with pagination support.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A paginated list of doctors.</returns>
        public async Task<List<TBL03>> GetAllDoctorsAsync(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                throw new AppException("Page number must be greater than 0.", StatusCodes.Status400BadRequest);
            }

            if (pageSize < 1)
            {
                throw new AppException("Page size must be greater than 0.", StatusCodes.Status400BadRequest);
            }

            using IDbConnection db = _dbFactory.Open();

            int skip = (page - 1) * pageSize;

            SqlExpression<TBL03> q = db.From<TBL03>()
                .Join<TBL03, TBL01>((d, u) => d.L03F02 == u.L01F01)
                .Skip(skip)
                .Limit(pageSize);

            List<Tuple<TBL03, TBL01>> doctors = await db.SelectMultiAsync<TBL03, TBL01>(q);

            return doctors.Select(x =>
            {
                TBL03 doctor = x.Item1;
                doctor.L03F07 = x.Item2;
                return doctor;
            }).ToList();


        }

        /// <inheritdoc/>
        public async Task<TBL01?> FindUserByIdAsync(int userId)
        {
            using IDbConnection db = _dbFactory.Open();
            return await db.SingleByIdAsync<TBL01>(userId);
        }

        /// <inheritdoc/>
        public async Task<bool> DoesDoctorExistAsync(int doctorUserId)
        {
            using IDbConnection db = _dbFactory.Open();
            return await db.ExistsAsync<TBL03>(d => d.L03F02 == doctorUserId);
        }

        /// <inheritdoc/>
        public async Task<int> CreateAppointmentAsync(TBL04 appointment)
        {
            using IDbConnection db = _dbFactory.Open();
            return (int)await db.InsertAsync(appointment, selectIdentity: true);
        }

        /// <inheritdoc/>
        public async Task<TBL04?> FindAppointmentByIdAsync(int appointmentId)
        {
            using IDbConnection db = _dbFactory.Open();
            return await db.SingleByIdAsync<TBL04>(appointmentId);
        }

        /// <inheritdoc/>
        public async Task<List<TBL04>> GetDoctorAppointmentsByStatusAsync(int doctorUserId, AppointmentStatus status)
        {
            using IDbConnection db = _dbFactory.Open();
            SqlExpression<TBL04> query = db.From<TBL04>()
                .Where(a => a.L04F03 == doctorUserId && a.L04F06 == status)
                .OrderBy(a => a.L04F04);
            return await db.SelectAsync(query);
        }

        /// <inheritdoc/>
        public async Task UpdateAppointmentAsync(TBL04 appointment)
        {
            using IDbConnection db = _dbFactory.Open();
            await db.UpdateAsync(appointment);
        }

        /// <summary>
        /// Validates doctor identity and ownership for a target appointment.
        /// </summary>
        /// <param name="doctorUserId">The doctor user identifier.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>A tuple containing appointment entity or validation error.</returns>
        private async Task<TBL04> ValidateDoctorAndGetOwnedAppointmentAsync(int doctorUserId, int appointmentId)
        {
            bool doctorExists = await DoesDoctorExistAsync(doctorUserId);
            if (!doctorExists)
            {
                throw new AppException("Doctor profile not found.", StatusCodes.Status400BadRequest);
            }

            TBL04? appointment = await FindAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new AppException("Appointment not found.", StatusCodes.Status404NotFound);
            }

            if (appointment.L04F03 != doctorUserId)
            {
                throw new AppException("You are not authorized to modify this appointment.", StatusCodes.Status403Forbidden);
            }

            return appointment;
        }

        #endregion
    }
}
