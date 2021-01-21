using Expensely.Application.Queries.Abstractions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Transactions.GetCurrentMonthTransactionSummary
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrentMonthTransactionSummaryQueryProcessor
        : IQueryProcessor<GetCurrentMonthTransactionSummaryQuery, Maybe<TransactionSummaryResponse>>
    {
    }
}
