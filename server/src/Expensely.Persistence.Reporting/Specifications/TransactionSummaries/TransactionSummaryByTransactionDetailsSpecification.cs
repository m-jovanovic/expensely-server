using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Persistence.Reporting.Specifications.TransactionSummaries
{
    /// <summary>
    /// Represents the specification for determining the transaction summary by transaction details.
    /// </summary>
    public sealed class TransactionSummaryByTransactionDetailsSpecification : Specification<TransactionSummary>
    {
        private readonly TransactionDetails _transactionDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummaryByTransactionDetailsSpecification"/> class.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        public TransactionSummaryByTransactionDetailsSpecification(TransactionDetails transactionDetails) =>
            _transactionDetails = transactionDetails;

        /// <inheritdoc />
        public override Expression<Func<TransactionSummary, bool>> ToExpression() =>
            transactionSummary =>
                transactionSummary.UserId == _transactionDetails.UserId &&
                transactionSummary.Year == _transactionDetails.OccurredOn.Year &&
                transactionSummary.Month == _transactionDetails.OccurredOn.Month &&
                transactionSummary.TransactionType == _transactionDetails.TransactionType &&
                transactionSummary.Currency == _transactionDetails.Currency;
    }
}
