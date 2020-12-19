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
    /// Represents the <see cref="ExpenseMoneyChangedEvent"/> handler, <see cref="IncomeMoneyChangedEvent"/> handler.
    /// </summary>
    public sealed class TransactionMoneyChangedEventHandler :
        IEventHandler<ExpenseMoneyChangedEvent>,
        IEventHandler<IncomeMoneyChangedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionMoneyChangedEventHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public TransactionMoneyChangedEventHandler(IReportingDbContext reportingDbContext, ITransactionSummaryAggregator transactionSummaryAggregator)
        {
            _reportingDbContext = reportingDbContext;
            _transactionSummaryAggregator = transactionSummaryAggregator;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseMoneyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionMoneyChangedAsync(@event.ExpenseId, @event.PreviousCurrency, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeMoneyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionMoneyChangedAsync(@event.IncomeId, @event.PreviousCurrency, cancellationToken);

        private async Task HandleTransactionMoneyChangedAsync(
            Guid transactionId,
            int previousCurrency,
            CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            await _transactionSummaryAggregator.AggregateForTransactionAsync(maybeTransaction.Value, previousCurrency, cancellationToken);
        }
    }
}
