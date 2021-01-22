using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Expenses;
using Expensely.Application.Queries.Processors.Expenses;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Persistence.QueryProcessors.Expenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> processor.
    /// </summary>
    internal sealed class GetExpensesQueryProcessor : IGetExpensesQueryProcessor
    {
        /// <inheritdoc />
        // TODO: Implement the actual query.
        public Task<Maybe<ExpenseListResponse>> Process(GetExpensesQuery query, CancellationToken cancellationToken = default) =>
            Task.FromResult(Maybe<ExpenseListResponse>.None);
    }
}
