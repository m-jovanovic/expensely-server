using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the transaction errors.
        /// </summary>
        public static class Transaction
        {
            /// <summary>
            /// Gets the transaction not found error.
            /// </summary>
            public static Error NotFound => new("Transaction.NotFound", "The transaction with the specified identifier was not found.");

            /// <summary>
            /// Gets the transaction identifier is required error.
            /// </summary>
            public static Error IdentifierIsRequired => new("Transaction.IdentifierIsRequired", "The transaction identifier is required.");

            /// <summary>
            /// Gets the transaction description is required error.
            /// </summary>
            public static Error DescriptionIsRequired => new(
                "Transaction.DescriptionIsRequired",
                "The transaction description is required.");

            /// <summary>
            /// Gets the transaction occurred on date is required error.
            /// </summary>
            public static Error OccurredOnDateIsRequired => new(
                "Transaction.OccurredOnDateIsRequired",
                "The date the transaction occurred on is required.");
        }
    }
}
