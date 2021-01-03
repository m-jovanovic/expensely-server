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
    /// Decreases the respective category transaction summary when an <see cref="IncomeDeletedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseCategoryTransactionSummaryOnIncomeDeletedEventHandler : IEventHandler<IncomeDeletedEvent>
    {
        private readonly ICategoryTransactionSummaryAggregator _categoryTransactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseCategoryTransactionSummaryOnIncomeDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="categoryTransactionSummaryAggregator">The category transaction summary aggregator.</param>
        public DecreaseCategoryTransactionSummaryOnIncomeDeletedEventHandler(
            ICategoryTransactionSummaryAggregator categoryTransactionSummaryAggregator) =>
            _categoryTransactionSummaryAggregator = categoryTransactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(IncomeDeletedEvent @event, CancellationToken cancellationToken) =>
         await _categoryTransactionSummaryAggregator.DecreaseByAmountAsync(
             new TransactionDetails
             {
                 UserId = @event.UserId,
                 Category = @event.Category,
                 Amount = @event.Amount,
                 Currency = @event.Currency,
                 OccurredOn = @event.OccurredOn,
                 TransactionType = (int)TransactionType.Income
             },
             cancellationToken);
    }
}
