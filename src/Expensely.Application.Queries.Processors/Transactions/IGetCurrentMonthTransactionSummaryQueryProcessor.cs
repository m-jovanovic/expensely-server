using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrentMonthTransactionSummaryQueryProcessor
        : IQueryProcessor<GetCurrentMonthTransactionSummaryQuery, Maybe<TransactionSummaryResponse>>
    {
    }
}
