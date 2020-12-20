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
    /// Represents the <see cref="ExpenseCurrencyChangedEvent"/> handler and <see cref="IncomeCurrencyChangedEvent"/> handler.
    /// </summary>
    public sealed class TransactionCurrencyChangedEventHandler :
        IEventHandler<ExpenseCurrencyChangedEvent>,
        IEventHandler<IncomeCurrencyChangedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionCurrencyChangedEventHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public TransactionCurrencyChangedEventHandler(
            IReportingDbContext reportingDbContext,
            ITransactionSummaryAggregator transactionSummaryAggregator)
        {
            _reportingDbContext = reportingDbContext;
            _transactionSummaryAggregator = transactionSummaryAggregator;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionCurrencyChangedAsync(@event.ExpenseId, @event.PreviousCurrency, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleTransactionCurrencyChangedAsync(@event.IncomeId, @event.PreviousCurrency, cancellationToken);

        private async Task HandleTransactionCurrencyChangedAsync(Guid transactionId, int currency, CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            Transaction transaction = maybeTransaction.Value.WithCurrency(currency);

            await _transactionSummaryAggregator.AggregateForTransactionAsync(transaction, cancellationToken);
        }
    }
}
