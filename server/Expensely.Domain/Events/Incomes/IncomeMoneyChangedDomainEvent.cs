using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the domain event that is raised when the monetary amount of an income is changed.
    /// </summary>
    public sealed class IncomeMoneyChangedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeMoneyChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="income">The income.</param>
        internal IncomeMoneyChangedDomainEvent(Income income)
            : base(Guid.NewGuid()) =>
            IncomeId = income.Id;

        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; }
    }
}
