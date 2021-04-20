using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Persistence.Indexes.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthExpensesPerCategoryQuery"/> processor.
    /// </summary>
    internal class GetCurrentMonthExpensesPerCategoryQueryProcessor : IGetCurrentMonthExpensesPerCategoryQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthExpensesPerCategoryQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetCurrentMonthExpensesPerCategoryQueryProcessor(
            IAsyncDocumentSession session,
            IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpensesPerCategoryResponse>> Process(
            GetCurrentMonthExpensesPerCategoryQuery query,
            CancellationToken cancellationToken = default)
        {
            if (query.UserId != _userInformationProvider.UserId)
            {
                return Maybe<ExpensesPerCategoryResponse>.None;
            }

            Transactions_Monthly.Result[] monthlyTransactions = await _session
                .Query<Transactions_Monthly.Result, Transactions_Monthly>()
                .Where(x =>
                    x.UserId == query.UserId &&
                    x.Year == query.StartOfMonth.Year &&
                    x.Month == query.StartOfMonth.Month &&
                    x.Currency == query.Currency &&
                    x.TransactionType == TransactionType.Expense.Value)
                .ToArrayAsync(cancellationToken);

            Currency currency = Currency.FromValue(query.Currency).Value;

            ExpensesPerCategoryResponse.ExpensePerCategoryItem[] expensesPerCategory = monthlyTransactions
                .Select(x => new ExpensesPerCategoryResponse.ExpensePerCategoryItem
                {
                    Category = Category.FromValue(x.Category).Value.Name,
                    FormattedAmount = currency.Format(x.Amount)
                }).ToArray();

            return new ExpensesPerCategoryResponse(expensesPerCategory);
        }
    }
}
