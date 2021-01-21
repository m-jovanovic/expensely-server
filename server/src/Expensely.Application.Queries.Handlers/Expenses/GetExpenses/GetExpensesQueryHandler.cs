using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Handlers.Expenses.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
        private readonly IGetExpensesQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get expenses query processor.</param>
        public GetExpensesQueryHandler(IGetExpensesQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
