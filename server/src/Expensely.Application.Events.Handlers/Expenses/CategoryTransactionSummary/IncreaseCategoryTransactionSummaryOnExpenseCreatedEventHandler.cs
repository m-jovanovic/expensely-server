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
    /// Increases the respective category transaction summary when an <see cref="ExpenseCreatedEvent"/> is raised.
    /// </summary>
    public sealed class IncreaseCategoryTransactionSummaryOnExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly ICategoryTransactionSummaryAggregator _categoryTransactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncreaseCategoryTransactionSummaryOnExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="categoryTransactionSummaryAggregator">The category transaction summary aggregator.</param>
        public IncreaseCategoryTransactionSummaryOnExpenseCreatedEventHandler(
            ICategoryTransactionSummaryAggregator categoryTransactionSummaryAggregator) =>
            _categoryTransactionSummaryAggregator = categoryTransactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseCreatedEvent @event, CancellationToken cancellationToken) =>
            await _categoryTransactionSummaryAggregator.IncreaseByAmountAsync(
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
