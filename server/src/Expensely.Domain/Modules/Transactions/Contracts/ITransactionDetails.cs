using System;
using Expensely.Domain.Modules.Common;

namespace Expensely.Domain.Modules.Transactions.Contracts
{
    /// <summary>
    /// Represents the transaction details interface.
    /// </summary>
    public interface ITransactionDetails
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        Description Description { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        Category Category { get; init; }

        /// <summary>
        /// Gets the money.
        /// </summary>
        Money Money { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        DateTime OccurredOn { get; init; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        TransactionType TransactionType { get; init; }
    }
}
