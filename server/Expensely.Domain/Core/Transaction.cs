using System;
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
        /// <param name="name">The name of the transaction.</param>
        /// <param name="money">The monetary amount of the transaction.</param>
        /// <param name="occurredOn">The date the transaction occurred on.</param>
        /// <param name="description">The description of the transaction.</param>
        protected Transaction(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
            : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(userId, "The user identifier is required.", nameof(userId));
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotEmpty(money, "The monetary amount is required.", nameof(money));
            Ensure.NotEmpty(occurredOn, "The occurred on date is required.", nameof(occurredOn));
            Ensure.NotEmpty(description, "The description is required.", nameof(description));

            UserId = userId;
            Money = money;
            OccurredOn = occurredOn.Date;
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
        /// Gets or sets the name.
        /// </summary>
        public Name Name { get; protected set; }

        /// <summary>
        /// Gets or sets the money.
        /// </summary>
        public Money Money { get; protected set; }

        /// <summary>
        /// Gets or sets the date the transaction occurred on.
        /// </summary>
        public DateTime OccurredOn { get; protected set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public Description Description { get; protected set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }
    }
}
