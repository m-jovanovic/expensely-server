using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;

namespace Expensely.Domain.Repositories
{
    /// <summary>
    /// Represents the user repository interface.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user with the specified identifier, if one exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the user with the specified identifier.</returns>
        Task<Maybe<User>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the user with the specified email, if one exists.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the user with the specified email.</returns>
        Task<Maybe<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any user already exists with the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if a user exists with the specified email, otherwise false.</returns>
        Task<bool> AnyWithEmailAsync(Email email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified user to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(User user, CancellationToken cancellationToken = default);
    }
}
