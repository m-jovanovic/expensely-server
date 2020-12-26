using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Events.Handlers.Incomes
{
    /// <summary>
    /// Represents the <see cref="IncomeCurrencyChangedEvent"/> handler.
    /// </summary>
    public sealed class IncomeCurrencyChangedEventHandler : IEventHandler<IncomeCurrencyChangedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeCurrencyChangedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public IncomeCurrencyChangedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeCurrencyChangedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.AggregateAsync(@event.IncomeId, @event.PreviousCurrency, cancellationToken);
    }
}
