using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Errors;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the transaction type enumeration.
    /// </summary>
    public abstract partial class TransactionType : Enumeration<TransactionType>
    {
        /// <summary>
        /// The expense transaction type.
        /// </summary>
        public static readonly TransactionType Expense = new ExpenseTransactionType();

        /// <summary>
        /// The income transaction type.
        /// </summary>
        public static readonly TransactionType Income = new IncomeTransactionType();

        /// <summary>
        /// Validates that the specified monetary amount is valid for the current transaction type.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <returns>The success result if the validation was successful, otherwise an error result.</returns>
        public abstract Result ValidateAmount(Money money);

        /// <summary>
        /// Represents the expense transaction type.
        /// </summary>
        private sealed class ExpenseTransactionType : TransactionType
        {
            /// <inheritdoc />
            public override Result ValidateAmount(Money money) =>
                money.Amount < decimal.Zero ?
                    Result.Success() :
                    Result.Failure(DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);
        }

        /// <summary>
        /// Represents the income transaction type.
        /// </summary>
        private sealed class IncomeTransactionType : TransactionType
        {
            /// <inheritdoc />
            public override Result ValidateAmount(Money money) =>
                money.Amount > decimal.Zero ?
                    Result.Success() :
                    Result.Failure(DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
