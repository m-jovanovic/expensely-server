using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions.Aggregation
{
    /// <summary>
    /// Represents the transaction summary aggregator interface.
    /// </summary>
    public interface ITransactionSummaryAggregator
    {
        /// <summary>
        /// Aggregates the transaction summary for the specified transaction identifier and optional currency.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AggregateAsync(Guid transactionId, int? currency = null, CancellationToken cancellationToken = default);
    }
}
