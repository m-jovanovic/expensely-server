using System;
using System.Collections.Generic;
using Expensely.Application.Contracts.Budgets;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Budgets
{
    /// <summary>
    /// Represents the query for getting a list of active budgets.
    /// </summary>
    public sealed class GetActiveBudgetsQuery : IQuery<IEnumerable<BudgetListItemResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetActiveBudgetsQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public GetActiveBudgetsQuery(Ulid userId) => UserId = userId;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }
    }
}
