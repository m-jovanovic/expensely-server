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
    /// Represents the <see cref="ExpenseAmountChangedEvent"/> handler.
    /// </summary>
    public sealed class ExpenseAmountChangedEventHandler : IEventHandler<ExpenseAmountChangedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseAmountChangedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public ExpenseAmountChangedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseAmountChangedEvent @event, CancellationToken cancellationToken) =>
            await _transactionSummaryAggregator.IncreaseByAmountAsync(
                new TransactionDetails
                {
                    UserId = @event.UserId,
                    Amount = @event.Amount - @event.PreviousAmount,
                    Currency = @event.Currency,
                    OccurredOn = @event.OccurredOn,
                    TransactionType = (int)TransactionType.Expense
                },
                cancellationToken);
    }
}
