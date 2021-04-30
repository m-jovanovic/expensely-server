using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the user errors.
        /// </summary>
        public static class User
        {
            /// <summary>
            /// Gets the user not found error.
            /// </summary>
            public static Error NotFound => new("User.NotFound", "The user with the specified identifier was not found.");

            /// <summary>
            /// Gets the user identifier is required error.
            /// </summary>
            public static Error IdentifierIsRequired => new("User.IdentifierIsRequired", "The user identifier is required.");

            /// <summary>
            /// Gets the user has invalid permissions error.
            /// </summary>
            public static Error InvalidPermissions => new(
                "User.InvalidPermissions",
                "The current user does not have sufficient permissions to perform this operation.");

            /// <summary>
            /// Gets the user email is required error.
            /// </summary>
            public static Error EmailIsRequired => new("User.EmailIsRequired", "The user email is required.");

            /// <summary>
            /// Gets the user first name is required error.
            /// </summary>
            public static Error FirstNameIsRequired => new("User.FirstNameIsRequired", "The user first name is required.");

            /// <summary>
            /// Gets the user last name is required error.
            /// </summary>
            public static Error LastNameIsRequired => new("User.LastNameIsRequired", "The user last name is required.");

            /// <summary>
            /// Gets the user password is required error.
            /// </summary>
            public static Error PasswordIsRequired => new("User.PasswordIsRequired", "The user password is required.");

            /// <summary>
            /// Gets the user password and confirmation password must match error.
            /// </summary>
            public static Error PasswordAndConfirmationPasswordMustMatch => new(
                "User.PasswordAndConfirmationPasswordMustMatch",
                "The provided password and confirmation password must match.");

            /// <summary>
            /// Gets the user email or password is invalid error.
            /// </summary>
            public static Error InvalidEmailOrPassword => new("User.InvalidEmailOrPassword", "The provided email or password is invalid.");
        }
    }
}
