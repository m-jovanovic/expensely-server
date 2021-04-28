using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Budgets;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetActiveBudgetsQuery"/> handler.
    /// </summary>
    internal sealed class GetActiveBudgetsQueryHandler : IQueryHandler<GetActiveBudgetsQuery, Maybe<IEnumerable<BudgetListItemResponse>>>
    {
        private readonly IGetActiveBudgetsQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetActiveBudgetsQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The query processor.</param>
        public GetActiveBudgetsQueryHandler(IGetActiveBudgetsQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<IEnumerable<BudgetListItemResponse>>> Handle(
            GetActiveBudgetsQuery request,
            CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
