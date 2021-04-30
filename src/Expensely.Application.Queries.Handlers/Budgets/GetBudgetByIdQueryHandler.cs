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
    /// Represents the <see cref="GetBudgetByIdQuery"/> handler.
    /// </summary>
    public sealed class GetBudgetByIdQueryHandler : IQueryHandler<GetBudgetByIdQuery, Maybe<BudgetResponse>>
    {
        private readonly IGetBudgetByIdQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBudgetByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get budget by identifier query processor.</param>
        public GetBudgetByIdQueryHandler(IGetBudgetByIdQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<BudgetResponse>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
