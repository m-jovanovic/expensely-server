using Expensely.Domain.Exceptions;
using Expensely.Domain.Primitives;
using Expensely.Shared.Primitives.Errors;

namespace Expensely.Domain.Modules.Budgets.Exceptions
{
    /// <summary>
    /// Represents the exception that is raised when there is an attempt to mark an already expired budget as expired.
    /// </summary>
    public sealed class BudgetAlreadyExpiredDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetAlreadyExpiredDomainException"/> class.
        /// </summary>
        public BudgetAlreadyExpiredDomainException()
            : base(new Error("Budget.AlreadyExpired", "The budget is already expired"))
        {
        }
    }
}
