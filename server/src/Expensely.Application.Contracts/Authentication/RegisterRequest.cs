namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the register request.
    /// </summary>
    public sealed class RegisterRequest
    {
        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Gets the confirmation password.
        /// </summary>
        public string ConfirmationPassword { get; init; }
    }
}
