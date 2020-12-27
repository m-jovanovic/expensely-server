using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Events.Expenses;

namespace Expensely.Application.Events.Handlers.Expenses
{
    /// <summary>
    /// Decreases the respective transaction summary when an <see cref="ExpenseUpdatedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseTransactionSummaryOnExpenseUpdatedEventHandler : IEventHandler<ExpenseUpdatedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseTransactionSummaryOnExpenseUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public DecreaseTransactionSummaryOnExpenseUpdatedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseUpdatedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.DecreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Amount = @event.PreviousAmount ?? @event.Amount,
                    Currency = @event.PreviousCurrency ?? @event.Currency,
                    OccurredOn = @event.PreviousOccurredOn ?? @event.OccurredOn,
                    TransactionType = (int)TransactionType.Expense
                },
                cancellationToken);
    }
}
