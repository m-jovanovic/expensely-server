using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Events.Handlers.Incomes
{
    /// <summary>
    /// Represents the <see cref="IncomeAmountChangedEvent"/> handler.
    /// </summary>
    public sealed class IncomeAmountChangedEventHandler : IEventHandler<IncomeAmountChangedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeAmountChangedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public IncomeAmountChangedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.AggregateAsync(@event.IncomeId, cancellationToken: cancellationToken);
    }
}
