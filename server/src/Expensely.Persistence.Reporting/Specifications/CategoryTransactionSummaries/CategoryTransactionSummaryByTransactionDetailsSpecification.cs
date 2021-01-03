using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Persistence.Reporting.Specifications.CategoryTransactionSummaries
{
    /// <summary>
    /// Represents the specification for determining the category transaction summary by transaction details.
    /// </summary>
    public sealed class CategoryTransactionSummaryByTransactionDetailsSpecification : Specification<CategoryTransactionSummary>
    {
        private readonly TransactionDetails _transactionDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTransactionSummaryByTransactionDetailsSpecification"/> class.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        public CategoryTransactionSummaryByTransactionDetailsSpecification(TransactionDetails transactionDetails) =>
            _transactionDetails = transactionDetails;

        /// <inheritdoc />
        public override Expression<Func<CategoryTransactionSummary, bool>> ToExpression() =>
            categoryTransactionSummary =>
                categoryTransactionSummary.UserId == _transactionDetails.UserId &&
                categoryTransactionSummary.Year == _transactionDetails.OccurredOn.Year &&
                categoryTransactionSummary.Month == _transactionDetails.OccurredOn.Month &&
                categoryTransactionSummary.TransactionType == _transactionDetails.TransactionType &&
                categoryTransactionSummary.Currency == _transactionDetails.Currency &&
                categoryTransactionSummary.Category == _transactionDetails.Category;
    }
}
