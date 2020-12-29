using System;

namespace Expensely.Contracts.Incomes
{
    /// <summary>
    /// Represents the create income request.
    /// </summary>
    public sealed class CreateIncomeRequest
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public int Category { get; init; }

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
