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
    public sealed class ExpenseUpdatedEventHandler : IEventHandler<ExpenseUpdatedEvent>
    {
        private readonly ITransactionSummaryAggregator _transactionSummaryAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseUpdatedEventHandler"/> class.
        /// </summary>
        /// <param name="transactionSummaryAggregator">The transaction summary aggregator.</param>
        public ExpenseUpdatedEventHandler(ITransactionSummaryAggregator transactionSummaryAggregator) =>
            _transactionSummaryAggregator = transactionSummaryAggregator;

        /// <inheritdoc />
        public async Task Handle(ExpenseUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var transactionDetails = new TransactionDetails
            {
                UserId = @event.UserId,
                Amount = @event.Amount,
                Currency = @event.Currency,
                OccurredOn = @event.OccurredOn,
                TransactionType = (int)TransactionType.Expense
            };

            await _transactionSummaryAggregator.IncreaseByAmountAsync(transactionDetails, cancellationToken);

            TransactionDetails previousTransactionDetails = GetPreviousTransactionDetails(@event, transactionDetails);

            await _transactionSummaryAggregator.DecreaseByAmountAsync(previousTransactionDetails, cancellationToken);
        }

        private static TransactionDetails GetPreviousTransactionDetails(ExpenseUpdatedEvent @event, TransactionDetails transactionDetails)
        {
            TransactionDetails previousTransactionDetails = transactionDetails;

            if (@event.PreviousAmount.HasValue)
            {
                previousTransactionDetails = previousTransactionDetails.WithAmount(@event.PreviousAmount.Value);
            }

            if (@event.PreviousCurrency.HasValue)
            {
                previousTransactionDetails = previousTransactionDetails.WithCurrency(@event.PreviousCurrency.Value);
            }

            if (@event.PreviousOccurredOn.HasValue)
            {
                previousTransactionDetails = previousTransactionDetails.WithOccurredOn(@event.PreviousOccurredOn.Value);
            }

            return previousTransactionDetails;
        }
    }
}
