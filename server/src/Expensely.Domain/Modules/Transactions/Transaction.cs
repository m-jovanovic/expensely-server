using System;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents a monetary transaction.
    /// </summary>
    public class Transaction : AggregateRoot, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <param name="money">The monetary amount.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="transactionType">The transaction type.</param>
        internal Transaction(
            User user,
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
            : base(Ulid.NewUlid())
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));
            Ensure.NotNull(description, "The description is required.", nameof(description));
            Ensure.NotNull(category, "The description is required.", nameof(category));
            Ensure.NotEmpty(money, "The description is required.", nameof(money));
            Ensure.NotEmpty(occurredOn, "The description is required.", nameof(occurredOn));
            Ensure.NotNull(transactionType, "The transaction type is required.", nameof(transactionType));

            UserId = Ulid.Parse(user.Id);
            Description = description;
            Category = category;
            Money = money;
            OccurredOn = occurredOn.Date;
            TransactionType = transactionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Transaction()
        {
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public Description Description { get; private set; }

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
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>
        /// Updates the transaction with the specified transaction details.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        public void Update(TransactionDetails transactionDetails)
        {
            ChangeDescriptionInternal(transactionDetails.Description);

            ChangeCategoryInternal(transactionDetails.Category);

            ChangeMoneyInternal(transactionDetails.Money);

            ChangeOccurredOnInternal(transactionDetails.OccurredOn);
        }

        /// <summary>
        /// Changes the description of the transaction.
        /// </summary>
        /// <param name="description">The new description.</param>
        private void ChangeDescriptionInternal(Description description)
        {
            Ensure.NotNull(description, "The description is required.", nameof(description));

            if (description == Description)
            {
                return;
            }

            Description = description;
        }

        /// <summary>
        /// Changes the category of the transaction.
        /// </summary>
        /// <param name="category">The new category.</param>
        private void ChangeCategoryInternal(Category category)
        {
            Ensure.NotNull(category, "The category is required", nameof(category));

            if (Category == category)
            {
                return;
            }

            Category = category;
        }

        /// <summary>
        /// Changes the monetary amount of the transaction.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        private void ChangeMoneyInternal(Money money)
        {
            Ensure.NotEmpty(money, "The monetary amount is required.", nameof(money));

            if (Money == money)
            {
                return;
            }

            Money = money;
        }

        /// <summary>
        /// Changes the occurred on date of the transaction.
        /// </summary>
        /// <param name="occurredOn">The new occurred on date.</param>
        private void ChangeOccurredOnInternal(DateTime occurredOn)
        {
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));

            if (OccurredOn == occurredOn)
            {
                return;
            }

            OccurredOn = occurredOn.Date;
        }
    }
}
