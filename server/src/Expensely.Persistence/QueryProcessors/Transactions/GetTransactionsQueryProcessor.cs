using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> processor.
    /// </summary>
    internal sealed class GetTransactionsQueryProcessor : IGetTransactionsQueryProcessor
    {
        /// <inheritdoc />
        // TODO: Implement the actual query.
        public Task<Maybe<TransactionListResponse>> Process(GetTransactionsQuery query, CancellationToken cancellationToken = default) =>
            Task.FromResult(Maybe<TransactionListResponse>.None);
    }
}
