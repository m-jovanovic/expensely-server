using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.Modules.Transactions.Events;
using Expensely.Domain.Modules.Transactions.Exceptions;
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
        private Transaction(
            User user,
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
            : base(Ulid.NewUlid())
        {
            EnsureValidDetails(user, description, category, money, occurredOn, transactionType);

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
        /// Creates a new transaction for the specified user and transaction details.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <returns>The newly created transaction.</returns>
        internal static Transaction Create(User user, ITransactionDetails transactionDetails)
        {
            var transaction = new Transaction(
                user,
                transactionDetails.Description,
                transactionDetails.Category,
                transactionDetails.Money,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType);

            transaction.Raise(new TransactionCreatedEvent
            {
                TransactionId = Ulid.Parse(transaction.Id)
            });

            return transaction;
        }

        /// <summary>
        /// Updates the transaction with the specified transaction details.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        public void ChangeDetails(ITransactionDetails transactionDetails)
        {
            EnsureValidTransactionDetails(
                transactionDetails.Description,
                transactionDetails.Category,
                transactionDetails.Money,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType);

            Description = transactionDetails.Description;
            Category = transactionDetails.Category;
            Money = transactionDetails.Money;
            OccurredOn = transactionDetails.OccurredOn;
        }

        private static void EnsureValidDetails(
            User user,
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));

            EnsureValidTransactionDetails(description, category, money, occurredOn, transactionType);
        }

        private static void EnsureValidTransactionDetails(
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
        {
            Ensure.NotNull(description, "The description is required.", nameof(description));
            Ensure.NotNull(category, "The category is required.", nameof(category));
            Ensure.NotNull(money, "The monetary amount is required.", nameof(money));
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));
            Ensure.NotNull(transactionType, "The transaction type is required.", nameof(transactionType));
            EnsureAmountValidForTransactionType(transactionType, money);
            EnsureCategoryValidForTransactionType(transactionType, category);
        }

        private static void EnsureAmountValidForTransactionType(TransactionType transactionType, Money money)
        {
            if (transactionType.ValidateAmount(money).IsFailure)
            {
                throw new AmountNotValidForTransactionTypeDomainException();
            }
        }

        private static void EnsureCategoryValidForTransactionType(TransactionType transactionType, Category category)
        {
            if (transactionType.ValidateCategory(category).IsFailure)
            {
                throw new CategoryNotValidForTransactionTypeDomainException();
            }
        }
    }
}
