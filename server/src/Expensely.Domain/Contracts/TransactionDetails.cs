using System;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Transactions;

namespace Expensely.Domain.Contracts
{
    /// <summary>
    /// Represents the transaction details.
    /// </summary>
    public class TransactionDetails
    {
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

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public TransactionType TransactionType { get; init; }
    }
}
