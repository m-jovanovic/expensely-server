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
            /// Gets the expense amount greater than zero error.
            /// </summary>
            internal static Error AmountGreaterThanZero => new Error(
                "Expense.AmountGreaterThanZero",
                "The expense amount can not be greater than zero.");

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
