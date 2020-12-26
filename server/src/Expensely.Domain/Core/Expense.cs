using System;
using Expensely.Domain.Events.Expenses;
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
        /// <param name="name">The name of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        private Expense(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
            : base(userId, name, money, occurredOn, description) =>
            EnsureMoneyIsLessThanZero(money);

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Expense()
        {
        }

        /// <summary>
        /// Creates a new expense based on the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        /// <returns>The newly created expense.</returns>
        public static Expense Create(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
        {
            var expense = new Expense(userId, name, money, occurredOn, description);

            expense.Raise(new ExpenseCreatedEvent
            {
                UserId = expense.UserId,
                Amount = expense.Money.Amount,
                Currency = expense.Money.Currency.Value,
                OccurredOn = expense.OccurredOn
            });

            return expense;
        }

        /// <summary>
        /// Changes the monetary amount of the expense.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        public void ChangeMoney(Money money)
        {
            EnsureMoneyIsLessThanZero(money);

            Money previousMoney = Money;

            (bool amountChanged, bool currencyChanged) = ChangeMoneyInternal(money);

            if (amountChanged)
            {
                Raise(new ExpenseAmountChangedEvent
                {
                    ExpenseId = Id
                });
            }

            if (currencyChanged)
            {
                Raise(new ExpenseCurrencyChangedEvent
                {
                    ExpenseId = Id,
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
        /// Ensures that the specified money amount is less than zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is greater than or equal to zero.</exception>
        private static void EnsureMoneyIsLessThanZero(Money money) =>
            Ensure.NotGreaterThanOrEqualToZero(money.Amount, "The monetary amount must be less than zero.", nameof(money));
    }
}
