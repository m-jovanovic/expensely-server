using System;
using Expensely.Domain.Contracts;
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
        private Expense(string userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
            : base(userId, name, category, money, occurredOn, description, TransactionType.Expense) =>
            EnsureMoneyIsLessThanZero(money);

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <param name="transactionInformation">The transaction information.</param>
        private Expense(TransactionInformation transactionInformation)
            : base(
                transactionInformation.UserId,
                transactionInformation.Name,
                transactionInformation.Category,
                transactionInformation.Money,
                transactionInformation.OccurredOn,
                transactionInformation.Description,
                TransactionType.Expense) =>
            EnsureMoneyIsLessThanZero(transactionInformation.Money);

        /// <summary>
        /// Initializes a new instance of the <see cref="Expense"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Expense()
        {
        }

        /// <summary>
        /// Creates a new <see cref="Expense"/> based on the specified parameters.
        /// </summary>
        /// <param name="transactionInformation">The transaction information.</param>
        /// <returns>The newly created expense.</returns>
        public static Expense Create(TransactionInformation transactionInformation)
        {
            var expense = new Expense(transactionInformation);

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
        /// Creates a new <see cref="Expense"/> based on the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the expense.</param>
        /// <param name="category">The category of the expense.</param>
        /// <param name="money">The monetary amount of the expense.</param>
        /// <param name="occurredOn">The date the expense occurred on.</param>
        /// <param name="description">The description of the expense.</param>
        /// <returns>The newly created expense.</returns>
        public static Expense Create(string userId, Name name, Category category, Money money, DateTime occurredOn, Description description)
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
        /// <param name="transactionInformation">The transaction information.</param>
        public void Update(TransactionInformation transactionInformation)
        {
            EnsureMoneyIsLessThanZero(transactionInformation.Money);

            ChangeNameInternal(transactionInformation.Name);

            ChangeDescriptionInternal(transactionInformation.Description);

            Category previousCategory = Category;

            bool categoryHasChanged = ChangeCategoryInternal(transactionInformation.Category);

            Money previousMoney = Money;

            (bool amountChanged, bool currencyChanged) = ChangeMoneyInternal(transactionInformation.Money);

            DateTime previousOccurredOn = OccurredOn;

            bool occurredOnChanged = ChangeOccurredOnInternal(transactionInformation.OccurredOn);

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
