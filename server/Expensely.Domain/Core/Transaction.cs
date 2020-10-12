using System;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Primitives;
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
        /// <param name="money">The monetary amount of the transaction.</param>
        /// <param name="transactionType">The transaction type.</param>
        protected Transaction(Guid userId, Money money, TransactionType transactionType)
            : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
            Ensure.NotEmpty(money, "The monetary amount is required.", nameof(money));

            UserId = userId;
            Money = money;
            TransactionType = transactionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected Transaction()
        {
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the money.
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }
    }
}
