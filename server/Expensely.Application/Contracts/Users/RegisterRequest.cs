namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the register request.
    /// </summary>
    public sealed class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
    }
}
