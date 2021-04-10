using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Budgets;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Budgets;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetTransactionByIdQuery"/> processor.
    /// </summary>
    internal sealed class GetBudgetDetailsByIdQueryProcessor : IGetBudgetDetailsByIdQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBudgetDetailsByIdQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetBudgetDetailsByIdQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<BudgetDetailsResponse>> Process(
            GetBudgetDetailsByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var budget = await _session
                .Query<Budget>()
                .Where(x => x.Id == query.BudgetId)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.Name,
                    x.Money,
                    Categories = x.Categories.Select(c => c.Name).ToArray(),
                    x.StartDate,
                    x.EndDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (budget is null || budget.UserId != _userInformationProvider.UserId)
            {
                return Maybe<BudgetDetailsResponse>.None;
            }

            // TODO: Implement getting remaining fields.
            var budgetDetailsResponse = new BudgetDetailsResponse
            {
                Id = budget.Id,
                Name = budget.Name,
                Amount = budget.Money.Format(),
                RemainingAmount = string.Empty,
                UsedPercentage = 0.0m,
                Categories = budget.Categories,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate
            };

            return budgetDetailsResponse;
        }
    }
}
