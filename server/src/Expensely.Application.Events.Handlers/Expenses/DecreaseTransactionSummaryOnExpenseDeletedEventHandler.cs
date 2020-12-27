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
    /// Decreases the respective transaction summary when an <see cref="ExpenseDeletedEvent"/> is raised.
    /// </summary>
    public sealed class DecreaseTransactionSummaryOnExpenseDeletedEventHandler : IEventHandler<ExpenseDeletedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecreaseTransactionSummaryOnExpenseDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public DecreaseTransactionSummaryOnExpenseDeletedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseDeletedEvent @event, CancellationToken cancellationToken) =>
         await _transactionSummaryAggregator.DecreaseByAmountAsync(
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
