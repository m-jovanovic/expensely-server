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
            /// Gets the expense category invalid error.
            /// </summary>
            public static Error ExpenseCategoryInvalid => new(
                "Transaction.ExpenseCategoryInvalid",
                "The provided category is not valid for an expense transaction.");

            /// <summary>
            /// Gets the income category invalid error.
            /// </summary>
            public static Error IncomeCategoryInvalid => new(
                "Transaction.IncomeCategoryInvalid ",
                "The provided category is not valid for an income transaction.");

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

            /// <summary>
            /// Gets the amount not valid for transaction type error.
            /// </summary>
            public static Error AmountNotValidForTransactionType => new(
                "Transaction.AmountNotValidForTransactionType",
                "The provided monetary amount is not valid for the current transaction type.");

            /// <summary>
            /// Gets the category not valid for transaction type error.
            /// </summary>
            public static Error CategoryNotValidForTransactionType => new(
                "Transaction.CategoryNotValidForTransactionType",
                "The provided category is not valid for the current transaction type.");
        }
    }
}
