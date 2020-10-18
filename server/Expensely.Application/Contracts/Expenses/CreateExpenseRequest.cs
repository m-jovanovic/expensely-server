using System;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the create expense request.
    /// </summary>
    public sealed class CreateExpenseRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency value.
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// Gets or sets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; set; }
    }
}
