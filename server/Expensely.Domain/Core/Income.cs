using System;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the income monetary transaction.
    /// </summary>
    public class Income : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="money">The monetary amount.</param>
        public Income(Guid userId, Money money)
            : base(userId, money, TransactionType.Income) =>
            Ensure.NotLessThanZero(money.Amount, "The monetary amount must be greater than or equal to zero", nameof(money));
    }
}
