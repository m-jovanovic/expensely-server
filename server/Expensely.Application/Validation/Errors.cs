using Expensely.Domain.Primitives;

namespace Expensely.Application.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static class Errors
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
            /// Gets the use email is already in use error.
            /// </summary>
            internal static Error EmailAlreadyInUse => new Error("User.EmailAlreadyInUse", "The specified email is already in use.");

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

            /// <summary>
            /// Gets the user email or password is invalid error.
            /// </summary>
            internal static Error InvalidEmailOrPassword => new Error("User.InvalidEmail", "The provided email or password is invalid.");
        }

        /// <summary>
        /// Contains the expense errors.
        /// </summary>
        internal static class Expense
        {
            /// <summary>
            /// Gets the expense identifier is required error.
            /// </summary>
            internal static Error IdentifierIsRequired => new Error("Expense.IdentifierIsRequired", "The expense identifier is required.");

            /// <summary>
            /// Gets the expense name is required error.
            /// </summary>
            internal static Error NameIsRequired => new Error("Expense.NameIsRequired", "The expense name is required.");

            /// <summary>
            /// Gets the expense amount greater than or equal to zero error.
            /// </summary>
            internal static Error AmountGreaterThanOrEqualToZero => new Error(
                "Expense.AmountGreaterThanOrEqualToZero",
                "The expense amount can not be greater than or equal to zero.");

            /// <summary>
            /// Gets the expense occurred on date is required error.
            /// </summary>
            internal static Error OccurredOnDateIsRequired => new Error(
                "Expense.OccurredOnDateIsRequired",
                "The date the expense occurred on is required.");

            /// <summary>
            /// Gets the expense not found error.
            /// </summary>
            internal static Error NotFound => new Error("Expense.NotFound", "The expense with the specified value was not found.");
        }

        /// <summary>
        /// Contains the currency errors.
        /// </summary>
        internal static class Currency
        {
            /// <summary>
            /// Gets the currency not found error.
            /// </summary>
            internal static Error NotFound => new Error("Currency.NotFound", "The currency with the specified value was not found.");
        }
    }
}
