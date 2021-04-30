using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetBudgetByIdQuery"/> processor interface.
    /// </summary>
    public interface IGetBudgetByIdQueryProcessor : IQueryProcessor<GetBudgetByIdQuery, Maybe<BudgetResponse>>
    {
    }
}
