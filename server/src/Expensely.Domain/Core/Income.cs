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
        /// <param name="category">The category of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        private Income(Guid userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
            : base(userId, name, category, money, occurredOn, description) =>
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
        /// Creates a new <see cref="Income"/> based on the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the income.</param>
        /// <param name="category">The category of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        /// <returns>The newly created income.</returns>
        public static Income Create(Guid userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
        {
            var income = new Income(userId, name, category, money, occurredOn, description);

            income.Raise(new IncomeCreatedEvent
            {
                UserId = income.UserId,
                Category = income.Category.Value,
                Amount = income.Money.Amount,
                Currency = income.Money.Currency.Value,
                OccurredOn = income.OccurredOn
            });

            return income;
        }

        /// <summary>
        /// Updates the income with the specified parameters.
        /// </summary>
        /// <param name="name">The name of the income.</param>
        /// <param name="category">The category of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        public void Update(Name name, Category category, Money money, DateTime occurredOn, Description description)
        {
            EnsureMoneyIsGreaterThanZero(money);

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

            Raise(new IncomeUpdatedEvent
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
        /// Ensures that the specified money amount is greater than zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is less than or equal to zero.</exception>
        private static void EnsureMoneyIsGreaterThanZero(Money money) =>
            Ensure.NotLessThanOrEqualToZero(money.Amount, "The monetary amount must be greater than zero.", nameof(money));
    }
}
