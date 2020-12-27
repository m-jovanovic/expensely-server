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
    /// Represents the <see cref="ExpenseUpdatedEvent"/> handler.
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
            await _transactionSummaryAggregator.DecreaseByAmountAsync(GetTransactionDetails(@event), cancellationToken);

        private static TransactionDetails GetTransactionDetails(ExpenseUpdatedEvent @event)
        {
            var transactionDetails = new TransactionDetails
            {
                UserId = @event.UserId,
                Amount = @event.Amount,
                Currency = @event.Currency,
                OccurredOn = @event.OccurredOn,
                TransactionType = (int)TransactionType.Expense
            };

            if (@event.PreviousAmount.HasValue)
            {
                transactionDetails = transactionDetails.WithAmount(@event.PreviousAmount.Value);
            }

            if (@event.PreviousCurrency.HasValue)
            {
                transactionDetails = transactionDetails.WithCurrency(@event.PreviousCurrency.Value);
            }

            if (@event.PreviousOccurredOn.HasValue)
            {
                transactionDetails = transactionDetails.WithOccurredOn(@event.PreviousOccurredOn.Value);
            }

            return transactionDetails;
        }
    }
}
