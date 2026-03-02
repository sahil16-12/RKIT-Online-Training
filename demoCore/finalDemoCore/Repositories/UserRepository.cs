using backend.Models;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace backend.Repositories
{
    /// <summary>
    /// Provides persistence operations for users, patients, and doctors.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        #region Private Fields

        /// <summary>
        /// Represents the database connection factory.
        /// </summary>
        private readonly IDbConnectionFactory _dbFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbFactory">The database connection factory.</param>
        public UserRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<int> CreateUserAsync(TBL01 user)
        {
            using var db = _dbFactory.Open();
            return (int)await db.InsertAsync(user, selectIdentity: true);
        }

        /// <inheritdoc/>
        public async Task CreatePatientAsync(TBL02 patient)
        {
            using var db = _dbFactory.Open();
            await db.InsertAsync(patient);
        }

        /// <inheritdoc/>
        public async Task CreateDoctorAsync(TBL03 doctor)
        {
            using var db = _dbFactory.Open();
            await db.InsertAsync(doctor);
        }

        /// <inheritdoc/>
        public async Task<TBL01?> FindUserByEmailAsync(string email)
        {
            using var db = _dbFactory.Open();
            return await db.SingleAsync<TBL01>(u => u.L01F04 == email);
        }

        /// <inheritdoc/>
        public async Task<TBL02?> FindPatientByUserIdAsync(int userId)
        {
            using var db = _dbFactory.Open();
            return await db.SingleAsync<TBL02>(p => p.L02F02 == userId);
        }

        /// <inheritdoc/>
        public async Task<TBL03?> FindDoctorByUserIdAsync(int userId)
        {
            using var db = _dbFactory.Open();
            return await db.SingleAsync<TBL03>(d => d.L03F02 == userId);
        }

        #endregion
    }
}
