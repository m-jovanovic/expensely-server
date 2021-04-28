using System.Collections.Generic;
using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetActiveBudgetsQuery"/> processor interface.
    /// </summary>
    public interface IGetActiveBudgetsQueryProcessor : IQueryProcessor<GetActiveBudgetsQuery, Maybe<IEnumerable<BudgetListItemResponse>>>
    {
    }
}
