using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Events.Expenses;

namespace Expensely.Application.Events.Handlers.Expenses.TransactionSummary
{
    /// <summary>
    /// Increases the respective transaction summary when an <see cref="ExpenseUpdatedEvent"/> is raised.
    /// </summary>
    public sealed class IncreaseTransactionSummaryOnExpenseUpdatedEventHandler : IEventHandler<ExpenseUpdatedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncreaseTransactionSummaryOnExpenseUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public IncreaseTransactionSummaryOnExpenseUpdatedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseUpdatedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.IncreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Amount = @event.Amount,
                    Currency = @event.Currency,
                    OccurredOn = @event.OccurredOn,
                    TransactionType = (int)TransactionType.Expense
                },
                cancellationToken);
    }
}
