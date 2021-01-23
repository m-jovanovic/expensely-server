using System;
using Expensely.Domain.Core;

namespace Expensely.Contracts.Expenses
{
    /// <summary>
    /// Represents the expense response.
    /// </summary>
    public sealed class ExpenseResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseResponse"/> class.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <param name="money">The money.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        public ExpenseResponse(string id, Money money, DateTime occurredOn, DateTime createdOnUtc)
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
