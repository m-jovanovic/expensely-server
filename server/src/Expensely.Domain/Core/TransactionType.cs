using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Errors;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the transaction type enumeration.
    /// </summary>
    public abstract class TransactionType : Enumeration<TransactionType>
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
        /// Initializes a new instance of the <see cref="TransactionType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected TransactionType(int value, string name)
            : base(value, name)
        {
        }

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
            /// <summary>
            /// Initializes a new instance of the <see cref="TransactionType.ExpenseTransactionType"/> class.
            /// </summary>
            public ExpenseTransactionType()
                : base(1, "Expense")
            {
            }

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
            /// <summary>
            /// Initializes a new instance of the <see cref="TransactionType.IncomeTransactionType"/> class.
            /// </summary>
            public IncomeTransactionType()
                : base(2, "Income")
            {
            }

            /// <inheritdoc />
            public override Result ValidateAmount(Money money) =>
                money.Amount > decimal.Zero ?
                    Result.Success() :
                    Result.Failure(DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
