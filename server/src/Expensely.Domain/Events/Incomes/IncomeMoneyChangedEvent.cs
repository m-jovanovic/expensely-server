using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the event that is raised when the monetary amount of an income is changed.
    /// </summary>
    public sealed class IncomeMoneyChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; init; }
    }
}
