﻿using Expensely.Domain.Primitives;

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
            internal static Error IdentifierIsRequired => new Error("User.IdentifierIsRequired", "The user identifier is required.");

            /// <summary>
            /// Gets the user has invalid permissions error.
            /// </summary>
            internal static Error InvalidPermissions => new Error(
                "User.InvalidPermissions",
                "The current user does not have sufficient permissions to perform this operation.");

            /// <summary>
            /// Gets the user email is required error.
            /// </summary>
            internal static Error EmailIsRequired => new Error("User.EmailIsRequired", "The user email is required.");

            /// <summary>
            /// Gets the user first name is required error.
            /// </summary>
            internal static Error FirstNameIsRequired => new Error("User.FirstNameIsRequired", "The user first name is required.");

            /// <summary>
            /// Gets the user last name is required error.
            /// </summary>
            internal static Error LastNameIsRequired => new Error("User.LastNameIsRequired", "The user last name is required.");

            /// <summary>
            /// Gets the user password is required error.
            /// </summary>
            internal static Error PasswordIsRequired => new Error("User.PasswordIsRequired", "The user password is required.");

            /// <summary>
            /// Gets the user password and confirmation password must match error.
            /// </summary>
            internal static Error PasswordAndConfirmationPasswordMustMatch => new Error(
                "User.PasswordAndConfirmationPasswordMustMatch",
                "The provided password and confirmation password must match.");
        }
    }
}
