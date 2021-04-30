namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the refresh token request.
    /// </summary>
    public sealed class RefreshTokenRequest
    {
        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; init; }
    }
}
