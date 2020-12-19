using System;
using Expensely.Domain.Events.Incomes;
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
        /// <param name="name">The name of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        private Income(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
            : base(userId, name, money, occurredOn, description) =>
            EnsureMoneyIsGreaterThanZero(money);

        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Income()
        {
        }

        /// <summary>
        /// Creates a new income based on the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        /// <returns>The newly created income.</returns>
        public static Income Create(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
        {
            var income = new Income(userId, name, money, occurredOn, description);

            income.Raise(new IncomeCreatedEvent
            {
                IncomeId = income.Id
            });

            return income;
        }

        /// <summary>
        /// Changes the monetary amount of the expense.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        public void ChangeMoney(Money money)
        {
            EnsureMoneyIsGreaterThanZero(money);

            Money previousMoney = Money;

            if (!ChangeMoneyInternal(money))
            {
                Raise(new IncomeMoneyChangedEvent
                {
                    IncomeId = Id,
                    PreviousCurrency = previousMoney.Currency.Value
                });
            }
        }

        /// <summary>
        /// Changes the name of the expense.
        /// </summary>
        /// <param name="name">The new name.</param>
        public void ChangeName(Name name) => ChangeNameInternal(name);

        /// <summary>
        /// Changes the description of the expense.
        /// </summary>
        /// <param name="description">The new description.</param>
        public void ChangeDescription(Description description) => ChangeDescriptionInternal(description);

        /// <summary>
        /// Changes the occurred on date of the expense.
        /// </summary>
        /// <param name="occurredOn">The new occurred on date.</param>
        public void ChangeOccurredOnDate(DateTime occurredOn) => ChangeOccurredOnDateInternal(occurredOn);

        /// <summary>
        /// Ensures that the specified money amount is greater than zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is less than or equal to zero.</exception>
        private static void EnsureMoneyIsGreaterThanZero(Money money) =>
            Ensure.NotLessThanOrEqualToZero(money.Amount, "The monetary amount must be greater than zero.", nameof(money));
    }
}
