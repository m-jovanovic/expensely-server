using Expensely.Application.Queries.Abstractions;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Expenses.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> processor interface.
    /// </summary>
    public interface IGetExpensesQueryProcessor : IQueryProcessor<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
    }
}
