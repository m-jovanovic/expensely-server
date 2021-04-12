using System;
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
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
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
                    x.Categories,
                    x.StartDate,
                    x.EndDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (budget is null || budget.UserId != _userInformationProvider.UserId)
            {
                return Maybe<BudgetDetailsResponse>.None;
            }

            int[] categoryValues = budget.Categories.Select(x => x.Value).ToArray();

            // TODO: Define index for this query.
            Money[] expenseAmounts = await _session.Query<Transaction>()
                .Where(x =>
                    x.UserId == budget.UserId &&
                    x.TransactionType.Value == TransactionType.Expense.Value &&
                    x.OccurredOn >= budget.StartDate &&
                    x.OccurredOn <= budget.EndDate &&
                    x.Money.Currency.Value == budget.Money.Currency.Value &&
                    (!categoryValues.Any() || x.Category.Value.In(categoryValues)))
                .Select(x => x.Money)
                .ToArrayAsync(cancellationToken);

            Money totalExpense = expenseAmounts.Any() ? Money.Sum(expenseAmounts) : new Money(0.0m, budget.Money.Currency);

            var budgetDetailsResponse = new BudgetDetailsResponse
            {
                Id = budget.Id,
                Name = budget.Name,
                Amount = budget.Money.Format(),
                RemainingAmount = (budget.Money + totalExpense).Format(),
                UsedPercentage = Math.Abs(totalExpense.Amount / budget.Money.Amount),
                Categories = budget.Categories.Select(category => category.Name).ToArray(),
                StartDate = budget.StartDate,
                EndDate = budget.EndDate
            };

            return budgetDetailsResponse;
        }
    }
}
