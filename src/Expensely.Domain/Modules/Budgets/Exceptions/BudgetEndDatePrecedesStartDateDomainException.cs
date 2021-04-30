using System;
using Expensely.Domain.Errors;
using Expensely.Domain.Exceptions;

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
            : base(DomainErrors.Budget.EndDatePrecedesStartDate(startDate, endDate))
        {
        }
    }
}
