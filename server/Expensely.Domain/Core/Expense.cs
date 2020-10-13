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
            AssertMoneyIsLessThanOrEqualToZero(money);

        /// <summary>
        /// Changes the monetary amount of the expense.
        /// </summary>
        /// <param name="money">The money amount.</param>
        public void ChangeMoney(Money money)
        {
            AssertMoneyIsLessThanOrEqualToZero(money);

            // TODO: Add domain event.
            Money = money;
        }

        /// <summary>
        /// Asserts that the specified money amount is less than or equal to zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is greater than zero.</exception>
        private static void AssertMoneyIsLessThanOrEqualToZero(Money money) =>
            Ensure.NotGreaterThanZero(money.Amount, "The monetary amount must be less than or equal to zero", nameof(money));
    }
}
