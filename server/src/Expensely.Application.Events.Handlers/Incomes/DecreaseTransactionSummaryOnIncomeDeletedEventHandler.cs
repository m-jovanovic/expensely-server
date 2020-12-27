using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Events.Handlers.Incomes
{
    /// <summary>
    /// Decreases the respective transaction summary when an <see cref="IncomeDeletedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseTransactionSummaryOnIncomeDeletedEventHandler : IEventHandler<IncomeDeletedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseTransactionSummaryOnIncomeDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public DecreaseTransactionSummaryOnIncomeDeletedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator)
            => _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeDeletedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.DecreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Amount = @event.Amount,
                    Currency = @event.Currency,
                    OccurredOn = @event.OccurredOn,
                    TransactionType = (int)TransactionType.Income
                },
                cancellationToken);
    }
}
