using System;

namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the authentication token response.
    /// </summary>
    public sealed class TokenResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenResponse"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="refreshTokenExpiresOnUtc">The refresh token expires on date and time.</param>
        public TokenResponse(string token, string refreshToken, DateTime refreshTokenExpiresOnUtc)
        {
            Token = token;
            RefreshToken = refreshToken;
            RefreshTokenExpiresOnUtc = refreshTokenExpiresOnUtc;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        /// Gets the refresh token expires on date and time in UTC format.
        /// </summary>
        public DateTime RefreshTokenExpiresOnUtc { get; }
    }
}
