using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Events.Handlers.Specifications.Transactions;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Events.Expenses;
using Expensely.Domain.Events.Incomes;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Events.Handlers.Transactions
{
    /// <summary>
    /// Represents the <see cref="ExpenseAmountChangedEvent"/> handler, <see cref="IncomeAmountChangedEvent"/> handler.
    /// </summary>
    public sealed class TransactionAmountChangedEventHandler :
        IEventHandler<ExpenseAmountChangedEvent>,
        IEventHandler<IncomeAmountChangedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionAmountChangedEventHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public TransactionAmountChangedEventHandler(
            IReportingDbContext reportingDbContext,
            ITransactionSummaryAggregator transactionSummaryAggregator)
        {
            _reportingDbContext = reportingDbContext;
            _transactionSummaryAggregator = transactionSummaryAggregator;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionAmountChangedAsync(@event.ExpenseId, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionAmountChangedAsync(@event.IncomeId, cancellationToken);

        private async Task HandleTransactionAmountChangedAsync(
            Guid transactionId,
            CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            await _transactionSummaryAggregator.AggregateForTransactionAsync(maybeTransaction.Value, cancellationToken);
        }
    }
}
