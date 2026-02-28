using backend.Models;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace backend.Repositories
{
    /// <summary>
    /// Provides persistence operations for appointment workflows.
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        #region Private Fields

        /// <summary>
        /// Represents database connection factory.
        /// </summary>
        private readonly IDbConnectionFactory _dbFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepository"/> class.
        /// </summary>
        /// <param name="dbFactory">The database connection factory.</param>
        public AppointmentRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<List<TBL03>> GetAllDoctorsAsync()
        {
            using var db = _dbFactory.Open();
            var doctors = await db.SelectAsync<TBL03>();
            
            if (doctors.Count == 0)
                return doctors;

            var userIds = doctors.Select(d => d.L03F02).ToList();
            var users = await db.SelectAsync<TBL01>(u => Sql.In(u.L01F01, userIds));
            var userDict = users.ToDictionary(u => u.L01F01);

            foreach (var doctor in doctors)
            {
                if (userDict.TryGetValue(doctor.L03F02, out var user))
                    doctor.L03F07 = user;
            }

            return doctors;
        }

        /// <inheritdoc/>
        public async Task<TBL01?> FindUserByIdAsync(int userId)
        {
            using var db = _dbFactory.Open();
            return await db.SingleByIdAsync<TBL01>(userId);
        }

        /// <inheritdoc/>
        public async Task<bool> DoesDoctorExistAsync(int doctorUserId)
        {
            using var db = _dbFactory.Open();
            return await db.ExistsAsync<TBL03>(d => d.L03F02 == doctorUserId);
        }

        /// <inheritdoc/>
        public async Task<bool> IsDoctorSlotOccupiedAsync(int doctorUserId, DateTime appointmentAtUtc)
        {
            using var db = _dbFactory.Open();
            
            const string sql = @"
                SELECT EXISTS(
                    SELECT 1 FROM appointments
                    WHERE doctor_user_id = @DoctorUserId
                      AND appointment_at_utc = @AppointmentAtUtc
                      AND status IN ('PENDING', 'APPROVED')
                )";

            return await db.ScalarAsync<bool>(sql, new 
            { 
                DoctorUserId = doctorUserId, 
                AppointmentAtUtc = appointmentAtUtc 
            });
        }

        /// <inheritdoc/>
        public async Task<bool> IsPatientDuplicateAppointmentAsync(int patientUserId, int doctorUserId, DateTime appointmentAtUtc)
        {
            using var db = _dbFactory.Open();
            
            const string sql = @"
                SELECT EXISTS(
                    SELECT 1 FROM appointments
                    WHERE patient_user_id = @PatientUserId
                      AND doctor_user_id = @DoctorUserId
                      AND appointment_at_utc = @AppointmentAtUtc
                      AND status IN ('PENDING', 'APPROVED')
                )";

            return await db.ScalarAsync<bool>(sql, new 
            { 
                PatientUserId = patientUserId,
                DoctorUserId = doctorUserId,
                AppointmentAtUtc = appointmentAtUtc 
            });
        }

        /// <inheritdoc/>
        public async Task<int> CreateAppointmentAsync(TBL04 appointment)
        {
            using var db = _dbFactory.Open();
            return (int)await db.InsertAsync(appointment, selectIdentity: true);
        }

        /// <inheritdoc/>
        public async Task<TBL04?> FindAppointmentByIdAsync(int appointmentId)
        {
            using var db = _dbFactory.Open();
            return await db.SingleByIdAsync<TBL04>(appointmentId);
        }

        /// <inheritdoc/>
        public async Task<List<TBL04>> GetDoctorAppointmentsByStatusAsync(int doctorUserId, AppointmentStatus status)
        {
            using var db = _dbFactory.Open();
            var query = db.From<TBL04>()
                .Where(a => a.L04F03 == doctorUserId && a.L04F06 == status)
                .OrderBy(a => a.L04F04);
            return await db.SelectAsync(query);
        }

        /// <inheritdoc/>
        public async Task UpdateAppointmentAsync(TBL04 appointment)
        {
            using var db = _dbFactory.Open();
            await db.UpdateAsync(appointment);
        }

        #endregion
    }
}
