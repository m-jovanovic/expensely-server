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
        /// <param name="category">The category of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        private Expense(Guid userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
            : base(userId, name, category, money, occurredOn, description) =>
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
        /// Creates a new <see cref="Expense"/> based on the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the expense.</param>
        /// <param name="category">The category of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        /// <returns>The newly created expense.</returns>
        public static Expense Create(Guid userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
        {
            var expense = new Expense(userId, name, category, money, occurredOn, description);

            expense.Raise(new ExpenseCreatedEvent
            {
                UserId = expense.UserId,
                Category = expense.Category.Value,
                Amount = expense.Money.Amount,
                Currency = expense.Money.Currency.Value,
                OccurredOn = expense.OccurredOn
            });

            return expense;
        }

        /// <summary>
        /// Updates the expense with the specified parameters.
        /// </summary>
        /// <param name="name">The name of the expense.</param>
        /// <param name="category">The category of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        public void Update(Name name, Category category, Money money, DateTime occurredOn, Description description)
        {
            EnsureMoneyIsLessThanZero(money);

            ChangeNameInternal(name);

            ChangeDescriptionInternal(description);

            Category previousCategory = Category;

            bool categoryHasChanged = ChangeCategoryInternal(category);

            Money previousMoney = Money;

            (bool amountChanged, bool currencyChanged) = ChangeMoneyInternal(money);

            DateTime previousOccurredOn = OccurredOn;

            bool occurredOnChanged = ChangeOccurredOnInternal(occurredOn);

            if (!categoryHasChanged && !amountChanged && !currencyChanged && !occurredOnChanged)
            {
                return;
            }

            Raise(new ExpenseUpdatedEvent
            {
                UserId = UserId,
                Category = Category.Value,
                PreviousCategory = categoryHasChanged ? previousCategory.Value : null,
                Amount = Money.Amount,
                PreviousAmount = amountChanged ? previousMoney.Amount : null,
                Currency = Money.Currency.Value,
                PreviousCurrency = currencyChanged ? previousMoney.Currency.Value : null,
                OccurredOn = OccurredOn,
                PreviousOccurredOn = occurredOnChanged ? previousOccurredOn : null
            });
        }

        /// <summary>
        /// Ensures that the specified money amount is less than zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is greater than or equal to zero.</exception>
        private static void EnsureMoneyIsLessThanZero(Money money) =>
            Ensure.NotGreaterThanOrEqualToZero(money.Amount, "The monetary amount must be less than zero.", nameof(money));
    }
}
