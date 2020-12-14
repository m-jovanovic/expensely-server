namespace Expensely.Contracts.Users
{
    /// <summary>
    /// Represents the change password request.
    /// </summary>
    public sealed class ChangePasswordRequest
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
