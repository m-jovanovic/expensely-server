using System;
using Expensely.Domain.Exceptions;
using Expensely.Domain.Primitives;
using Expensely.Shared.Primitives.Errors;

namespace Expensely.Domain.Modules.Budgets.Exceptions
{
    /// <summary>
    /// Represents the exception that is occurs when there is an attempt to instantiate a budget instance
    /// where the end date precedes the start date.
    /// </summary>
    public sealed class BudgetEndDatePrecedesStartDateDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetEndDatePrecedesStartDateDomainException"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public BudgetEndDatePrecedesStartDateDomainException(DateTime startDate, DateTime endDate)
            : base(new Error("Budget.EndDatePrecedesStartDate", $"The end date {endDate:d} precedes the start date {startDate:d}."))
        {
        }
    }
}
