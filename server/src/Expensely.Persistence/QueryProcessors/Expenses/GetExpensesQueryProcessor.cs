using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Queries.Expenses;
using Expensely.Application.Queries.Processors.Expenses;
using Expensely.Application.Queries.Utility;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Expenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> processor.
    /// </summary>
    internal sealed class GetExpensesQueryProcessor : IGetExpensesQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetExpensesQueryProcessor(IAsyncDocumentSession session, IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Process(GetExpensesQuery query, CancellationToken cancellationToken = default)
        {
            if (query.UserId != _userInformationProvider.UserId)
            {
                return Maybe<ExpenseListResponse>.None;
            }

            // TODO: Add index.
            ExpenseResponse[] expenses = await _session
                .Query<Transaction>()
                .Where(x =>
                    x.TransactionType == TransactionType.Expense &&
                    x.UserId == query.UserId &&
                    (x.OccurredOn < query.OccurredOn ||
                     x.OccurredOn == query.OccurredOn && x.CreatedOnUtc <= query.CreatedOnUtc))
                .OrderByDescending(x => x.OccurredOn)
                .ThenByDescending(x => x.CreatedOnUtc)
                .Select(x => new ExpenseResponse(x.Id, x.Money.Amount, x.Money.Currency.Value, x.OccurredOn, x.CreatedOnUtc))
                .ToArrayAsync(cancellationToken);

            if (expenses.Length < query.Limit)
            {
                return new ExpenseListResponse(expenses);
            }

            ExpenseResponse lastExpense = expenses[^1];

            string cursor = Cursor.Create(
                lastExpense.OccurredOn.ToString(DateTimeFormats.Date, CultureInfo.InvariantCulture),
                lastExpense.CreatedOnUtc.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            return new ExpenseListResponse(expenses[..^1], cursor);
        }
    }
}
