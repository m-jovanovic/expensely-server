using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
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
            /// Gets the expense transaction amount greater than or equal to zero error.
            /// </summary>
            public static Error ExpenseAmountGreaterThanOrEqualToZero => new(
                "Transaction.AmountGreaterThanOrEqualToZero",
                "The expense transaction amount can not be greater than or equal to zero.");

            /// <summary>
            /// Gets the income transaction amount less than or equal to zero error.
            /// </summary>
            public static Error IncomeAmountLessThanOrEqualToZero => new(
                "Transaction.AmountLessThanOrEqualToZero",
                "The income transaction amount can not be less than or equal to zero.");
        }
    }
}
