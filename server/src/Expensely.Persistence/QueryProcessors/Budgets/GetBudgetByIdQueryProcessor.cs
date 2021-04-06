using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Budgets;
using Expensely.Application.Queries.Processors.Budgets;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Budgets;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Budgets
{
    /// <summary>
    /// Represents the <see cref="GetTransactionByIdQuery"/> processor.
    /// </summary>
    internal sealed class GetBudgetByIdQueryProcessor : IGetBudgetByIdQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBudgetByIdQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetBudgetByIdQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<BudgetResponse>> Process(GetBudgetByIdQuery query, CancellationToken cancellationToken = default)
        {
            Maybe<Budget> maybeBudget = await _session.LoadAsync<Budget>(query.BudgetId, cancellationToken);

            if (maybeBudget.HasNoValue || maybeBudget.Value.UserId != _userInformationProvider.UserId)
            {
                return Maybe<BudgetResponse>.None;
            }

            Budget budget = maybeBudget.Value;

            var budgetResponse = new BudgetResponse
            {
                Id = budget.Id,
                Name = budget.Name,
                FormattedAmount = budget.Money.Format(),
                Amount = budget.Money.Amount,
                Currency = budget.Money.Currency.Value,
                Categories = budget.Categories.Select(category => new CategoryResponse
                {
                    Id = category.Value,
                    Name = category.ToString(),
                    IsDefault = category.IsDefault,
                    IsExpense = category.IsExpense
                }).ToArray(),
                StartDate = budget.StartDate,
                EndDate = budget.EndDate
            };

            return budgetResponse;
        }
    }
}
