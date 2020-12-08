using System;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;

namespace Expensely.Domain.Events.Expenses
{
    /// <summary>
    /// Represents the domain event that is raised when the monetary amount of an expense is changed.
    /// </summary>
    public sealed class ExpenseMoneyChangedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseMoneyChangedDomainEvent"/> class.
        /// </summary>
        /// <param name="expense">The expense.</param>
        internal ExpenseMoneyChangedDomainEvent(Expense expense) => ExpenseId = expense.Id;

        /// <summary>
        /// Gets the expense.
        /// </summary>
        public Guid ExpenseId { get; }
    }
}
