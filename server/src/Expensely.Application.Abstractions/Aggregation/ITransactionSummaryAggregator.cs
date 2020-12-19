using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Abstractions.Aggregation
{
    /// <summary>
    /// Represents the transaction summary aggregator interface.
    /// </summary>
    public interface ITransactionSummaryAggregator
    {
        /// <summary>
        /// Aggregates the transaction summary for the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AggregateForTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aggregates the transaction summary for the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="previousCurrency">The previous currency.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AggregateForTransactionAsync(Transaction transaction, int previousCurrency, CancellationToken cancellationToken = default);
    }
}
