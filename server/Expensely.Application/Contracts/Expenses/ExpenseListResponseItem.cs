using System;
using Expensely.Domain.Core;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the expense list response item.
    /// </summary>
    public sealed class ExpenseListResponseItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseListResponseItem"/> class.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <param name="amount">The monetary amount.</param>
        /// <param name="currencyValue">The currency identifier.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="createdOnUtc">The created on date and time in UTC format.</param>
        public ExpenseListResponseItem(Guid id, decimal amount, int currencyValue, DateTime occurredOn, DateTime createdOnUtc)
        {
            Id = id;

            Currency currency = Currency.FromValue(currencyValue).Value;

            Value = $"{amount} {currency.Code}";

            OccurredOn = occurredOn;

            CreatedOnUtc = createdOnUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the formatted expense value.
        /// </summary>
        public string Value { get; }

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
