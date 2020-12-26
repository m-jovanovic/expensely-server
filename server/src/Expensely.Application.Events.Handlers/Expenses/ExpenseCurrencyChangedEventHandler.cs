﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Events.Expenses;

namespace Expensely.Application.Events.Handlers.Expenses
{
    /// <summary>
    /// Represents the <see cref="ExpenseAmountChangedEvent"/> handler.
    /// </summary>
    public sealed class ExpenseCurrencyChangedEventHandler : IEventHandler<ExpenseCurrencyChangedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCurrencyChangedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public ExpenseCurrencyChangedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.AggregateAsync(@event.ExpenseId, @event.PreviousCurrency, cancellationToken);
    }
}
