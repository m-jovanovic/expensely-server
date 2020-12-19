using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Events.Handlers.Specifications.Transactions;
using Expensely.Application.Events.Handlers.Specifications.TransactionSummaries;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Events.Expenses;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Events.Handlers.Expenses
{
    /// <summary>
    /// Represents the <see cref="ExpenseCreatedEvent"/> handler.
    /// </summary>
    public sealed class ExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public ExpenseCreatedEventHandler(
            IReportingDbContext reportingDbContext,
            ITransactionSummaryAggregator transactionSummaryAggregator)
        {
            _reportingDbContext = reportingDbContext;
            _transactionSummaryAggregator = transactionSummaryAggregator;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseCreatedEvent @event, CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(@event.ExpenseId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            Transaction transaction = maybeTransaction.Value;

            bool transactionSummaryExists = await _reportingDbContext
                .AnyAsync(new TransactionSummaryByTransactionSpecification(transaction), cancellationToken);

            if (transactionSummaryExists)
            {
                await _transactionSummaryAggregator.AggregateForTransactionAsync(transaction, cancellationToken);

                return;
            }

            await InsertTransactionSummaryAsync(transaction, cancellationToken);
        }

        private async Task InsertTransactionSummaryAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            var transactionSummary = new TransactionSummary
            {
                Id = Guid.NewGuid(),
                UserId = transaction.UserId,
                Year = transaction.OccurredOn.Year,
                Month = transaction.OccurredOn.Month,
                TransactionType = transaction.TransactionType,
                Currency = transaction.Currency,
                Amount = transaction.Amount
            };

            _reportingDbContext.Insert(transactionSummary);

            await _reportingDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
