using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> processor.
    /// </summary>
    internal sealed class GetCurrentMonthTransactionSummaryQueryProcessor : IGetCurrentMonthTransactionSummaryQueryProcessor
    {
        /// <inheritdoc />
        // TODO: Implement the actual query using a map-reduce index.
        public Task<Maybe<TransactionSummaryResponse>> Process(
            GetCurrentMonthTransactionSummaryQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Maybe<TransactionSummaryResponse>.None);
    }
}
