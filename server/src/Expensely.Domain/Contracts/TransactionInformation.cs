using System;
using Expensely.Domain.Core;

namespace Expensely.Domain.Contracts
{
    /// <summary>
    /// Represents the transaction information.
    /// </summary>
    public class TransactionInformation
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public Name Name { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public Description Description { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public Category Category { get; init; }

        /// <summary>
        /// Gets the money.
        /// </summary>
        public Money Money { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }
    }
}
