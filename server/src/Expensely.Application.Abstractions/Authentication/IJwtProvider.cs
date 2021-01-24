using Expensely.Domain.Core;

namespace Expensely.Application.Abstractions.Authentication
{
    /// <summary>
    /// Represents the JWT provider interface.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Creates a new JWT for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT for the specified user.</returns>
        string CreateToken(User user);

        /// <summary>
        /// Creates a new refresh token.
        /// </summary>
        /// <returns>The refresh token and the expires on date and time in UTC format.</returns>
        RefreshToken CreateRefreshToken();
    }
}
