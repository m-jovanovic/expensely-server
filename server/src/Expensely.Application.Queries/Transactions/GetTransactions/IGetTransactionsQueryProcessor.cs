using Expensely.Application.Queries.Abstractions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Transactions.GetTransactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> processor interface.
    /// </summary>
    public interface IGetTransactionsQueryProcessor : IQueryProcessor<GetTransactionsQuery, Maybe<TransactionListResponse>>
    {
    }
}
