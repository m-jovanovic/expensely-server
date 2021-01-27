using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the transaction errors.
        /// </summary>
        internal static class Transaction
        {
            /// <summary>
            /// Gets the transaction identifier is required error.
            /// </summary>
            internal static Error IdentifierIsRequired => new("Transaction.IdentifierIsRequired", "The transaction identifier is required.");

            /// <summary>
            /// Gets the transaction name is required error.
            /// </summary>
            internal static Error NameIsRequired => new("Transaction.NameIsRequired", "The transaction name is required.");

            /// <summary>
            /// Gets the transaction occurred on date is required error.
            /// </summary>
            internal static Error OccurredOnDateIsRequired => new(
                "Transaction.OccurredOnDateIsRequired",
                "The date the transaction occurred on is required.");
        }
    }
}
