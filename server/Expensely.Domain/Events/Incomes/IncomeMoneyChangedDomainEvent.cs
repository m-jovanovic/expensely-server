using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Incomes
{
    /// <summary>
    /// Represents the domain event that is raised when the monetary amount of an income is changed.
    /// </summary>
    public sealed class IncomeMoneyChangedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeMoneyChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="income">The income.</param>
        /// <param name="previousMoney">The previous money amount.</param>
        internal IncomeMoneyChangedDomainEvent(Income income, Money previousMoney)
        {
            Income = income;
            PreviousMoney = previousMoney;
        }

        /// <summary>
        /// Gets the income.
        /// </summary>
        public Income Income { get; }

        /// <summary>
        /// Gets the previous money amount.
        /// </summary>
        public Money PreviousMoney { get; }
    }
}
