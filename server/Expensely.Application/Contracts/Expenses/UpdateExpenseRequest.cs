using System;

namespace Expensely.Application.Contracts.Expenses
{
    /// <summary>
    /// Represents the update expense request.
    /// </summary>
    public sealed class UpdateExpenseRequest
    {
        /// <summary>
        /// Gets or sets the expense identifier.
        /// </summary>
        public Guid ExpenseId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}
