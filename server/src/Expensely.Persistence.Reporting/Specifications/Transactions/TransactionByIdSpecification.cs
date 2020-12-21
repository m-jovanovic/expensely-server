using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Persistence.Reporting.Specifications.Transactions
{
    /// <summary>
    /// Represents the specification for determining the transaction by identifier.
    /// </summary>
    public sealed class TransactionByIdSpecification : Specification<Transaction>
    {
        private readonly Guid _transactionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionByIdSpecification"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        public TransactionByIdSpecification(Guid transactionId) => _transactionId = transactionId;

        /// <inheritdoc />
        public override Expression<Func<Transaction, bool>> ToExpression() => transaction => transaction.Id == _transactionId;
    }
}
