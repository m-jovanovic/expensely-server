using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Events.Incomes;

namespace Expensely.Application.Events.Handlers.Incomes.CategoryTransactionSummary
{
    /// <summary>
    /// Decreases the respective category transaction summary when an <see cref="IncomeUpdatedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseCategoryTransactionSummaryOnIncomeUpdatedEventHandler : IEventHandler<IncomeUpdatedEvent>
    {
        private readonly ICategoryTransactionSummaryAggregator _categoryTransactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseCategoryTransactionSummaryOnIncomeUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="categoryTransactionSummaryAggregator">The category transaction summary aggregator.</param>
        public DecreaseCategoryTransactionSummaryOnIncomeUpdatedEventHandler(
            ICategoryTransactionSummaryAggregator categoryTransactionSummaryAggregator) =>
            _categoryTransactionSummaryAggregator = categoryTransactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeUpdatedEvent @event, CancellationToken cancellationToken) =>
            await _categoryTransactionSummaryAggregator.DecreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Category = @event.PreviousCategory ?? @event.Category,
                    Amount = @event.PreviousAmount ?? @event.Amount,
                    Currency = @event.PreviousCurrency ?? @event.Currency,
                    OccurredOn = @event.PreviousOccurredOn ?? @event.OccurredOn,
                    TransactionType = (int)TransactionType.Income
                },
                cancellationToken);
    }
}
