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
    /// Decreases the respective category transaction summary when an <see cref="ExpenseUpdatedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseCategoryTransactionSummaryOnExpenseUpdatedEventHandler : IEventHandler<ExpenseUpdatedEvent>
    {
        private readonly ICategoryTransactionSummaryAggregator _categoryTransactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseCategoryTransactionSummaryOnExpenseUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="categoryTransactionSummaryAggregator">The category transaction summary aggregator.</param>
        public DecreaseCategoryTransactionSummaryOnExpenseUpdatedEventHandler(
            ICategoryTransactionSummaryAggregator categoryTransactionSummaryAggregator) =>
            _categoryTransactionSummaryAggregator = categoryTransactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseUpdatedEvent @event, CancellationToken cancellationToken) =>
            await _categoryTransactionSummaryAggregator.DecreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Category = @event.PreviousCategory ?? @event.Category,
                    Amount = @event.PreviousAmount ?? @event.Amount,
                    Currency = @event.PreviousCurrency ?? @event.Currency,
                    OccurredOn = @event.PreviousOccurredOn ?? @event.OccurredOn,
                    TransactionType = (int)TransactionType.Expense
                },
                cancellationToken);
    }
}
