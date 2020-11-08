using Expensely.Domain.Primitives;

namespace Expensely.Application.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class Errors
    {
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
            internal static Error NotFound => new Error("Expense.NotFound", "The expense with the specified identifier was not found.");
        }
    }
}
