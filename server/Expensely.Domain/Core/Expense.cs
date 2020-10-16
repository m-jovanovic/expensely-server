using System;
using Expensely.Domain.Events;
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
        /// <param name="occurredOn">The date the expense occurred on.</param>
        public Expense(Guid userId, Money money, DateTime occurredOn)
            : base(userId, money, occurredOn) =>
            AssertMoneyIsLessThanOrEqualToZero(money);

        /// <summary>
        /// Changes the monetary amount of the expense.
        /// </summary>
        /// <param name="money">The money amount.</param>
        public void ChangeMoney(Money money)
        {
            AssertMoneyIsLessThanOrEqualToZero(money);

            if (Money == money)
            {
                return;
            }

            Money = money;

            AddDomainEvent(new ExpenseMoneyChangedDomainEvent(this));
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
