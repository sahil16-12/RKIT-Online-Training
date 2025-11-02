using ServiceStack.OrmLite;
using System.Data;

namespace ReadingRoom.Api.Data
{

    /// <summary>
    /// Provides a connection factory for MySQL database access.
    /// </summary>
    public class DbConnectionFactory
    {
        private readonly OrmLiteConnectionFactory _factory;

        public DbConnectionFactory(string connectionString)
        {
            _factory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
        }

        /// <summary>
        /// Opens a new database connection.
        /// </summary>
        public IDbConnection Open()
        {
            return _factory.OpenDbConnection();
        }
    }
}
