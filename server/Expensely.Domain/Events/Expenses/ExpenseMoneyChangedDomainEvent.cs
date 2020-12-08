using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the domain event that is raised when the monetary amount of an expense is changed.
    /// </summary>
    public sealed class ExpenseMoneyChangedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseMoneyChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="expense">The expense.</param>
        /// <param name="previousMoney">The previous money amount.</param>
        internal ExpenseMoneyChangedDomainEvent(Expense expense, Money previousMoney)
            : base(Guid.NewGuid())
        {
            ExpenseId = expense.Id;
            PreviousMoney = previousMoney;
        }

        /// <summary>
        /// Gets the expense.
        /// </summary>
        public Guid ExpenseId { get; }

        /// <summary>
        /// Gets the previous money amount.
        /// </summary>
        public Money PreviousMoney { get; }
    }
}
