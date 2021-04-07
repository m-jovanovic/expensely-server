namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the JWT configuration settings.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Gets the issuer.
        /// </summary>
        public string Issuer { get; init; }

        /// <summary>
        /// Gets the audience.
        /// </summary>
        public string Audience { get; init; }

        /// <summary>
        /// Gets the security key.
        /// </summary>
        public string SecurityKey { get; init; }

        /// <summary>
        /// Gets the token expiration time in minutes.
        /// </summary>
        public int AccessTokenExpirationInMinutes { get; init; }

        /// <summary>
        /// Gets the refresh token expiration in minutes.
        /// </summary>
        public int RefreshTokenExpirationInMinutes { get; init; }
    }
}
