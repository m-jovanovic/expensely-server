using Expensely.Application.Contracts.Authentication;
using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the JWT provider interface.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Creates the access tokens for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The access tokens for the specified user.</returns>
        AccessTokens CreateAccessTokens(User user);
    }
}
