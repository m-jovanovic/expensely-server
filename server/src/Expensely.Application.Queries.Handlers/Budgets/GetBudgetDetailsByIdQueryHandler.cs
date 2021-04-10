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
    /// Represents the <see cref="GetBudgetDetailsByIdQuery"/> handler.
    /// </summary>
    public sealed class GetBudgetDetailsByIdQueryHandler : IQueryHandler<GetBudgetDetailsByIdQuery, Maybe<BudgetDetailsResponse>>
    {
        private readonly IGetBudgetDetailsByIdQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBudgetDetailsByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get budget by identifier query processor.</param>
        public GetBudgetDetailsByIdQueryHandler(IGetBudgetDetailsByIdQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<BudgetDetailsResponse>> Handle(GetBudgetDetailsByIdQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
