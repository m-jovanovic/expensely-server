namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the login request.
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; init; }
    }
}
