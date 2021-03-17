﻿using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Transactions
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

        private sealed class ExpenseTransactionType : TransactionType
        {
            public ExpenseTransactionType()
                : base(1, nameof(Expense))
            {
            }

            public override Result ValidateAmount(Money money) =>
                money.Amount < decimal.Zero ?
                    Result.Success() :
                    Result.Failure(DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);
        }

        private sealed class IncomeTransactionType : TransactionType
        {
            public IncomeTransactionType()
                : base(2, nameof(Income))
            {
            }

            public override Result ValidateAmount(Money money) =>
                money.Amount > decimal.Zero ?
                    Result.Success() :
                    Result.Failure(DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
