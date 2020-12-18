using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Events.Handlers.Specifications.TransactionSummaries
{
    /// <summary>
    /// Represents the specification for determining the transaction summary by transaction.
    /// </summary>
    public sealed class TransactionSummaryByTransactionSpecification : Specification<TransactionSummary>
    {
        private readonly Transaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummaryByTransactionSpecification"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public TransactionSummaryByTransactionSpecification(Transaction transaction) => _transaction = transaction;

        /// <inheritdoc />
        public override Expression<Func<TransactionSummary, bool>> ToExpression() =>
            transactionSummary =>
                transactionSummary.UserId == _transaction.UserId &&
                transactionSummary.Year == _transaction.OccurredOn.Year &&
                transactionSummary.Month == _transaction.OccurredOn.Month &&
                transactionSummary.TransactionType == _transaction.TransactionType &&
                transactionSummary.Currency == _transaction.Currency;
    }
}
