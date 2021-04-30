using System;
using Expensely.Domain.Modules.Common;

namespace Expensely.Domain.Modules.Budgets.Contracts
{
    /// <summary>
    /// Represents the budget details interface.
    /// </summary>
    public interface IBudgetDetails
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        Name Name { get; init; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        Category[] Categories { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        Money Money { get; init; }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        DateTime StartDate { get; init; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        DateTime EndDate { get; init; }
    }
}
