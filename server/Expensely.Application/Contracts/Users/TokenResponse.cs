namespace Expensely.Application.Contracts.Users
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
        public TokenResponse(string token) => Token = token;

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; }
    }
}
