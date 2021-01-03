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
    /// Increases the respective transaction summary when an <see cref="ExpenseCreatedEvent"/> is raised.
    /// </summary>
    public sealed class IncreaseTransactionSummaryOnExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncreaseTransactionSummaryOnExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public IncreaseTransactionSummaryOnExpenseCreatedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseCreatedEvent @event, CancellationToken cancellationToken) =>
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
