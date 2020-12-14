using System.Data;

namespace Expensely.Application.Abstractions.Data
{
    /// <summary>
    /// Represents the database connection provider interface.
    /// </summary>
    public interface IDbConnectionProvider
    {
        /// <summary>
        /// Creates a new database connection and returns it.
        /// </summary>
        /// <returns>The newly created database connection.</returns>
        IDbConnection Create();
    }
}
