﻿using System;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Contracts;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents a monetary transaction.
    /// </summary>
    public class Transaction : AggregateRoot, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        public Transaction(TransactionDetails transactionDetails)
            : base(Guid.NewGuid())
        {
            Ensure.NotNull(transactionDetails, "The transaction details are required.", nameof(transactionDetails));

            UserId = transactionDetails.UserId;
            Description = transactionDetails.Description;
            Category = transactionDetails.Category;
            Money = transactionDetails.Money;
            OccurredOn = transactionDetails.OccurredOn.Date;
            TransactionType = transactionDetails.TransactionType;
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
        public string UserId { get; private set; }

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
