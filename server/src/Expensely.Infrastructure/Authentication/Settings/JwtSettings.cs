namespace Expensely.Infrastructure.Authentication.Settings
{
    /// <summary>
    /// Represents the JWT configuration settings.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// The settings key.
        /// </summary>
        public const string SettingsKey = "Jwt";

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
