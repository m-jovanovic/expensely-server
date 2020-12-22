using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Events.Handlers.Incomes
{
    /// <summary>
    /// Represents the <see cref="IncomeCreatedEvent"/> handler.
    /// </summary>
    public sealed class IncomeCreatedEventHandler : IEventHandler<IncomeCreatedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public IncomeCreatedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeCreatedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.IncrementByTransactionAmountAsync(@event.IncomeId, cancellationToken);
    }
}
