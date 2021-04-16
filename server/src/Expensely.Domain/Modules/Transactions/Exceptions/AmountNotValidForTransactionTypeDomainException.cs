using Expensely.Domain.Errors;
using Expensely.Domain.Exceptions;

namespace Expensely.Domain.Modules.Transactions.Exceptions
{
    /// <summary>
    /// Represents the exception that is raised when there is an attempt to change the transaction amount
    /// to an invalid amount for that transaction type.
    /// </summary>
    public sealed class AmountNotValidForTransactionTypeDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmountNotValidForTransactionTypeDomainException"/> class.
        /// </summary>
        public AmountNotValidForTransactionTypeDomainException()
            : base(DomainErrors.Transaction.AmountNotValidForTransactionType)
        {
        }
    }
}
