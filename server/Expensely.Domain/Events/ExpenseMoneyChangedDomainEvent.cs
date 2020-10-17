﻿using Expensely.Domain.Core;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Events
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
        internal ExpenseMoneyChangedDomainEvent(Expense expense) => Expense = expense;

        /// <summary>
        /// Gets the expense.
        /// </summary>
        public Expense Expense { get; }
    }
}