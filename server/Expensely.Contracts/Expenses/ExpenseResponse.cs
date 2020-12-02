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
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        public ExpenseResponse(Guid id, decimal amount, int currency, DateTime occurredOn, DateTime createdOnUtc)
        {
            Id = id;

            FormattedExpense = Currency.FromValue(currency).Value.Format(amount);

            OccurredOn = occurredOn;

            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the formatted expense.
        /// </summary>
        public string FormattedExpense { get; }

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
