using Expensely.Application.Queries.Expenses;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Processors.Expenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> processor interface.
    /// </summary>
    public interface IGetExpensesQueryProcessor : IQueryProcessor<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
    }
}
