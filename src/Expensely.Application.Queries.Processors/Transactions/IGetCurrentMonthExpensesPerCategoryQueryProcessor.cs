﻿using System.Collections.Generic;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Processors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthExpensesPerCategoryQuery"/> processor interface.
    /// </summary>
    public interface IGetCurrentMonthExpensesPerCategoryQueryProcessor
        : IQueryProcessor<GetCurrentMonthExpensesPerCategoryQuery, Maybe<IEnumerable<ExpensePerCategoryResponse>>>
    {
    }
}
