using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Users.Contracts;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the user factory interface.
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Creates a new user based on the specified <see cref="CreateUserRequest"/> instance.
        /// </summary>
        /// <param name="createUserRequest">The create user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the user creation process containing the user or an error.</returns>
        Task<Result<User>> CreateAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default);
    }
}
