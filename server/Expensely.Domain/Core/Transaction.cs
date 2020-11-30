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
            Ensure.NotNull(description, "The description is required.", nameof(description));

            UserId = userId;
            Name = name;
            Money = money;
            OccurredOn = occurredOn.Date;
            Description = description;
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
        /// Gets the name.
        /// </summary>
        public Name Name { get; private set; }

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

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Changes the monetary amount of the transaction.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        protected void ChangeMoneyInternal(Money money)
        {
            if (Money == money)
            {
                return;
            }

            Money = money;
        }

        /// <summary>
        /// Changes the name of the transaction.
        /// </summary>
        /// <param name="name">The new name.</param>
        protected void ChangeNameInternal(Name name)
        {
            if (name == Name)
            {
                return;
            }

            Name = name;
        }

        /// <summary>
        /// Changes the description of the transaction.
        /// </summary>
        /// <param name="description">The new description.</param>
        protected void ChangeDescriptionInternal(Description description)
        {
            if (description == Description)
            {
                return;
            }

            Description = description;
        }

        /// <summary>
        /// Changes the occurred on date of the transaction.
        /// </summary>
        /// <param name="occurredOn">The new occurred on date.</param>
        protected void ChangeOccurredOnDateInternal(DateTime occurredOn)
        {
            if (OccurredOn == occurredOn)
            {
                return;
            }

            OccurredOn = occurredOn.Date;
        }
    }
}
