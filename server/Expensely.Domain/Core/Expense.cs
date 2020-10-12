using System;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the expense monetary transaction.
    /// </summary>
    public class Expense : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="money">The monetary amount.</param>
        public Expense(Guid userId, Money money)
            : base(userId, money, TransactionType.Expense) =>
            Ensure.NotGreaterThanZero(money.Amount, "The monetary amount must be less than zero", nameof(money));
    }
}
