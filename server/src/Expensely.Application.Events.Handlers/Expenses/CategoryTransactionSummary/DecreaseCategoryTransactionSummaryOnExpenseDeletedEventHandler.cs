using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Events.Expenses;

namespace Expensely.Application.Events.Handlers.Expenses.CategoryTransactionSummary
{
    /// <summary>
    /// Decreases the respective category transaction summary when an <see cref="ExpenseDeletedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseCategoryTransactionSummaryOnExpenseDeletedEventHandler : IEventHandler<ExpenseDeletedEvent>
    {
        private readonly ICategoryTransactionSummaryAggregator _categoryTransactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseCategoryTransactionSummaryOnExpenseDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="categoryTransactionSummaryAggregator">The category transaction summary aggregator.</param>
        public DecreaseCategoryTransactionSummaryOnExpenseDeletedEventHandler(
            ICategoryTransactionSummaryAggregator categoryTransactionSummaryAggregator) =>
            _categoryTransactionSummaryAggregator = categoryTransactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseDeletedEvent @event, CancellationToken cancellationToken) =>
         await _categoryTransactionSummaryAggregator.DecreaseByAmountAsync(
             new TransactionDetails
             {
                 UserId = @event.UserId,
                 Category = @event.Category,
                 Amount = @event.Amount,
                 Currency = @event.Currency,
                 OccurredOn = @event.OccurredOn,
                 TransactionType = (int)TransactionType.Expense
             },
             cancellationToken);
    }
}
