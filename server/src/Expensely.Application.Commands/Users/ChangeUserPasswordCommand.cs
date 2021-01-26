using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for changing a user's password.
    /// </summary>
    public sealed class ChangeUserPasswordCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPasswordCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The password.</param>
        /// <param name="confirmationPassword">The confirmation password.</param>
        public ChangeUserPasswordCommand(Guid userId, string currentPassword, string newPassword, string confirmationPassword)
        {
            UserId = userId.ToString();
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
            ConfirmationPassword = confirmationPassword;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the current password.
        /// </summary>
        public string CurrentPassword { get; }

        /// <summary>
        /// Gets the new password.
        /// </summary>
        public string NewPassword { get; }

        /// <summary>
        /// Gets the confirmation password.
        /// </summary>
        public string ConfirmationPassword { get; }
    }
}
