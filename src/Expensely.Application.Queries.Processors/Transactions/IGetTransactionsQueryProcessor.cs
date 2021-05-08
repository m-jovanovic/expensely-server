using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Application.Queries.Transactions;

namespace Expensely.Application.Queries.Processors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> processor interface.
    /// </summary>
    public interface IGetTransactionsQueryProcessor : IQueryProcessor<GetTransactionsQuery, TransactionListResponse>
    {
    }
}
