using Expensely.Domain.Errors;
using Expensely.Domain.Exceptions;

namespace Expensely.Domain.Modules.Transactions.Exceptions
{
    /// <summary>
    /// Represents the exception that is raised when there is an attempt to change the transaction category
    /// to an invalid category for that transaction type.
    /// </summary>
    public sealed class CategoryNotValidForTransactionTypeDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryNotValidForTransactionTypeDomainException"/> class.
        /// </summary>
        public CategoryNotValidForTransactionTypeDomainException()
            : base(DomainErrors.Transaction.CategoryNotValidForTransactionType)
        {
        }
    }
}
