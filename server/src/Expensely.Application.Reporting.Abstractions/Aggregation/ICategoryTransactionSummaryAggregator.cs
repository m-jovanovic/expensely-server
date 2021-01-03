using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Reporting.Abstractions.Contracts;

namespace Expensely.Application.Reporting.Abstractions.Aggregation
{
    /// <summary>
    /// Represents the category transaction summary aggregator interface.
    /// </summary>
    public interface ICategoryTransactionSummaryAggregator
    {
        /// <summary>
        /// Increases the respective category transaction summary amount based on the specified transaction details.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task IncreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default);

        /// <summary>
        /// Decreases the respective category transaction summary amount based on the specified transaction details.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task DecreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default);
    }
}
