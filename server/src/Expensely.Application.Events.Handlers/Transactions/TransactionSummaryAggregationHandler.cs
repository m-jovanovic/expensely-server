using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Events.Handlers.Specifications.Transactions;
using Expensely.Application.Events.Handlers.Specifications.TransactionSummaries;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Events.Expenses;
using Expensely.Domain.Events.Incomes;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Events.Handlers.Transactions
{
    /// TODO: Move all of the common logic into some sort of service? And then split all of the handlers into separate classes.
    /// <summary>
    /// Represents the transaction summary aggregation handler.
    /// </summary>
    public sealed class TransactionSummaryAggregationHandler :
        IEventHandler<ExpenseCreatedEvent>,
        IEventHandler<ExpenseAmountChangedEvent>,
        IEventHandler<ExpenseCurrencyChangedEvent>,
        IEventHandler<IncomeCreatedEvent>,
        IEventHandler<IncomeAmountChangedEvent>,
        IEventHandler<IncomeCurrencyChangedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummaryAggregationHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        /// <param name="dateTime">The date and time.</param>
        public TransactionSummaryAggregationHandler(
            IReportingDbContext reportingDbContext,
            ITransactionSummaryAggregator transactionSummaryAggregator,
            IDateTime dateTime)
        {
            _reportingDbContext = reportingDbContext;
            _transactionSummaryAggregator = transactionSummaryAggregator;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseCreatedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAsync(@event.ExpenseId, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(ExpenseAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAsync(@event.ExpenseId, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(ExpenseCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAndCurrencyAsync(@event.ExpenseId, @event.PreviousCurrency, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeCreatedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAsync(@event.IncomeId, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAsync(@event.IncomeId, cancellationToken);

        /// <inheritdoc />
        public async Task Handle(IncomeCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await HandleForTransactionIdAndCurrencyAsync(@event.IncomeId, @event.PreviousCurrency, cancellationToken);

        private async Task HandleForTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            await AggregateTransactionSummaryAsync(maybeTransaction.Value, cancellationToken);
        }

        private async Task HandleForTransactionIdAndCurrencyAsync(Guid transactionId, int currency, CancellationToken cancellationToken)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            Transaction transactionWithCurrency = maybeTransaction.Value.WithCurrency(currency);

            await AggregateTransactionSummaryAsync(transactionWithCurrency, cancellationToken);
        }

        private async Task AggregateTransactionSummaryAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            bool transactionSummaryExists = await _reportingDbContext
                .AnyAsync(new TransactionSummaryByTransactionSpecification(transaction), cancellationToken);

            if (transactionSummaryExists)
            {
                await _transactionSummaryAggregator.AggregateForTransactionAsync(transaction, cancellationToken);

                return;
            }

            await InsertTransactionSummaryAsync(transaction, _dateTime.UtcNow, cancellationToken);
        }

        private async Task InsertTransactionSummaryAsync(Transaction transaction, DateTime utcNow, CancellationToken cancellationToken)
        {
            var transactionSummary = new TransactionSummary
            {
                Id = Guid.NewGuid(),
                UserId = transaction.UserId,
                Year = transaction.OccurredOn.Year,
                Month = transaction.OccurredOn.Month,
                Currency = transaction.Currency,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                CreatedOnUtc = utcNow
            };

            _reportingDbContext.Insert(transactionSummary);

            await _reportingDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
