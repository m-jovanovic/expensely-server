using System;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents a monetary transaction.
    /// </summary>
    public abstract class Transaction : AggregateRoot, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the transaction.</param>
        /// <param name="category">The category of the transaction.</param>
        /// <param name="money">The monetary amount of the transaction.</param>
        /// <param name="occurredOn">The date the transaction occurred on.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="transactionType">The transaction type.</param>
        protected Transaction(
            string userId,
            Name name,
            Category category,
            Money money,
            DateTime occurredOn,
            Description description,
            TransactionType transactionType)
            : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotNull(category, "The category is required", nameof(category));
            Ensure.NotEmpty(money, "The monetary amount is required.", nameof(money));
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));
            Ensure.NotNull(description, "The description is required.", nameof(description));

            UserId = userId.ToString();
            Name = name;
            Category = category;
            Money = money;
            OccurredOn = occurredOn.Date;
            Description = description;
            TransactionType = transactionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        protected Transaction()
        {
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public Name Name { get; private set; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public Category Category { get; private set; }

        /// <summary>
        /// Gets the money.
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// Gets the date the transaction occurred on.
        /// </summary>
        public DateTime OccurredOn { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public Description Description { get; private set; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Changes the name of the transaction.
        /// </summary>
        /// <param name="name">The new name.</param>
        protected void ChangeNameInternal(Name name)
        {
            Ensure.NotEmpty(name, "The name is required.", nameof(name));

            if (name == Name)
            {
                return;
            }

            Name = name;
        }

        /// <summary>
        /// Changes the category of the transaction.
        /// </summary>
        /// <param name="category">The new category.</param>
        /// <returns>True if the category has been changed, otherwise false.</returns>
        protected bool ChangeCategoryInternal(Category category)
        {
            Ensure.NotNull(category, "The category is required", nameof(category));

            if (Category == category)
            {
                return false;
            }

            Category = category;

            return true;
        }

        /// <summary>
        /// Changes the monetary amount of the transaction.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        /// <returns>A tuple representing whether or not the amount and currency have changed.</returns>
        protected (bool AmountChanged, bool CurrencyChanged) ChangeMoneyInternal(Money money)
        {
            Ensure.NotEmpty(money, "The monetary amount is required.", nameof(money));

            if (Money == money)
            {
                return (false, false);
            }

            bool amountsAreDifferent = money.Amount != Money.Amount;

            bool currenciesAreDifferent = money.Currency != Money.Currency;

            Money = money;

            return (amountsAreDifferent, currenciesAreDifferent);
        }

        /// <summary>
        /// Changes the occurred on date of the transaction.
        /// </summary>
        /// <param name="occurredOn">The new occurred on date.</param>
        /// <returns>True if the occurred on date has been changed, otherwise false.</returns>
        protected bool ChangeOccurredOnInternal(DateTime occurredOn)
        {
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));

            if (OccurredOn == occurredOn)
            {
                return false;
            }

            OccurredOn = occurredOn.Date;

            return true;
        }

        /// <summary>
        /// Changes the description of the transaction.
        /// </summary>
        /// <param name="description">The new description.</param>
        protected void ChangeDescriptionInternal(Description description)
        {
            Ensure.NotNull(description, "The description is required.", nameof(description));

            if (description == Description)
            {
                return;
            }

            Description = description;
        }
    }
}
