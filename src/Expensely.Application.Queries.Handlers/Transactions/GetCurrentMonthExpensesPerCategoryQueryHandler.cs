using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthExpensesPerCategoryQuery"/> handler.
    /// </summary>
    internal sealed class GetCurrentMonthExpensesPerCategoryQueryHandler
        : IQueryHandler<GetCurrentMonthExpensesPerCategoryQuery, Maybe<ExpensesPerCategoryResponse>>
    {
        private readonly IGetCurrentMonthExpensesPerCategoryQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthExpensesPerCategoryQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The query processor.</param>
        public GetCurrentMonthExpensesPerCategoryQueryHandler(IGetCurrentMonthExpensesPerCategoryQueryProcessor processor) =>
            _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<ExpensesPerCategoryResponse>> Handle(
            GetCurrentMonthExpensesPerCategoryQuery request,
            CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
