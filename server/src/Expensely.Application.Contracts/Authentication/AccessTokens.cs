using Expensely.Domain.Modules.Users;

namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the access tokens.
    /// </summary>
    public sealed record AccessTokens(string Token, RefreshToken RefreshToken)
    {
        /// <summary>
        /// Creates a new token response.
        /// </summary>
        /// <returns>The new token response instance.</returns>
        public TokenResponse CreateTokenResponse() => new(Token, RefreshToken.Token, RefreshToken.ExpiresOnUtc);
    }
}
