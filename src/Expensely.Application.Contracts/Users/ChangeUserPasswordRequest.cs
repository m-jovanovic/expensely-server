namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the change user password request.
    /// </summary>
    public sealed class ChangeUserPasswordRequest
    {
        /// <summary>
        /// Gets the current password.
        /// </summary>
        public string CurrentPassword { get; init; }

        /// <summary>
        /// Gets the new password.
        /// </summary>
        public string NewPassword { get; init; }

        /// <summary>
        /// Gets the confirmation password.
        /// </summary>
        public string ConfirmationPassword { get; init; }
    }
}
