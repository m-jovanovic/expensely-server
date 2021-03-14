using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user factory interface.
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Creates a new user based on the specified parameters.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the user creation process containing the user or an error.</returns>
        Task<Result<User>> Create(
            string firstName,
            string lastName,
            string email,
            string password,
            CancellationToken cancellationToken = default);
    }
}
