using System.Data;
using Expensely.Application.Abstractions.Data;
using Expensely.Persistence.Configuraiton;
using Microsoft.Data.SqlClient;

namespace Expensely.Persistence.Providers
{
    /// <summary>
    /// Represents the database connection provider.
    /// </summary>
    internal sealed class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly ConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbConnectionProvider(ConnectionString connectionString) => _connectionString = connectionString;

        /// <inheritdoc />
        public IDbConnection Create()
        {
            var dbConnection = new SqlConnection(_connectionString.Value);

            dbConnection.Open();

            return dbConnection;
        }
    }
}
