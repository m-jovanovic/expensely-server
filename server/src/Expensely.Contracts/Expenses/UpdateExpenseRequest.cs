using System;

namespace Expensely.Contracts.Expenses
{
    /// <summary>
    /// Represents the update expense request.
    /// </summary>
    public sealed class UpdateExpenseRequest
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int Category { get; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; init; }
    }
}
