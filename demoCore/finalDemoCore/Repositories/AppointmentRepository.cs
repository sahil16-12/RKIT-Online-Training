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
        #endregion
    }
}
