using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the user errors.
        /// </summary>
        internal static class User
        {
            /// <summary>
            /// Gets the user identifier is required error.
            /// </summary>
            internal static Error IdentifierIsRequired => new("User.IdentifierIsRequired", "The user identifier is required.");

            /// <summary>
            /// Gets the user has invalid permissions error.
            /// </summary>
            internal static Error InvalidPermissions => new(
                "User.InvalidPermissions",
                "The current user does not have sufficient permissions to perform this operation.");

            /// <summary>
            /// Gets the user email is required error.
            /// </summary>
            internal static Error EmailIsRequired => new("User.EmailIsRequired", "The user email is required.");

            /// <summary>
            /// Gets the user first name is required error.
            /// </summary>
            internal static Error FirstNameIsRequired => new("User.FirstNameIsRequired", "The user first name is required.");

            /// <summary>
            /// Gets the user last name is required error.
            /// </summary>
            internal static Error LastNameIsRequired => new("User.LastNameIsRequired", "The user last name is required.");

            /// <summary>
            /// Gets the user password is required error.
            /// </summary>
            internal static Error PasswordIsRequired => new("User.PasswordIsRequired", "The user password is required.");

            /// <summary>
            /// Gets the user password and confirmation password must match error.
            /// </summary>
            internal static Error PasswordAndConfirmationPasswordMustMatch => new(
                "User.PasswordAndConfirmationPasswordMustMatch",
                "The provided password and confirmation password must match.");
        }
    }
}
