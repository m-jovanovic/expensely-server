using System;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the event that is raised when the currency of an income is changed.
    /// </summary>
    public class IncomeCurrencyChangedEvent : IEvent
    {
        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; init; }

        /// <summary>
        /// Gets the previous currency.
        /// </summary>
        public int PreviousCurrency { get; init; }
    }
}
