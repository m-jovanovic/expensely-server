using System;
using Expensely.Domain.Core;

namespace Expensely.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public sealed class TransactionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionResponse"/> class.
        /// </summary>
        /// <param name="id">The transaction identifier.</param>
        /// <param name="money">The money amount.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        public TransactionResponse(string id, Money money, DateTime occurredOn, DateTime createdOnUtc)
        {
            Id = id;
            FormattedAmount = money.Format();
            OccurredOn = occurredOn;
            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the created on date time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; }
    }
}
