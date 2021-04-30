using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetBudgetDetailsByIdQuery"/> processor interface.
    /// </summary>
    public interface IGetBudgetDetailsByIdQueryProcessor : IQueryProcessor<GetBudgetDetailsByIdQuery, Maybe<BudgetDetailsResponse>>
    {
    }
}
