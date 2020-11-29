using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Queries.Expenses.Queries.GetExpenses;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Messaging;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Queries.Handlers.Expenses.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetExpensesQueryHandler(IDbContext dbContext, IUserInformationProvider userInformationProvider)
        {
            _dbContext = dbContext;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Maybe<ExpenseListResponse>.None;
            }

            ExpenseListResponseItem[] expenses = await _dbContext.Set<Expense>()
                .Where(e => e.UserId == request.UserId &&
                            e.OccurredOn < request.OccurredOn ||
                            e.OccurredOn == request.OccurredOn && e.CreatedOnUtc <= request.CreatedOnUtc)
                .OrderByDescending(e => e.OccurredOn)
                .ThenByDescending(e => e.CreatedOnUtc)
                .Select(e => new ExpenseListResponseItem(e.Id, e.Money.Amount, e.Money.Currency.Value, e.OccurredOn, e.CreatedOnUtc))
                .Take(request.Limit)
                .ToArrayAsync(cancellationToken);

            if (expenses.Length < request.Limit)
            {
                return new ExpenseListResponse(expenses);
            }

            ExpenseListResponseItem lastExpense = expenses[^1];

            string cursor = Cursor.Create(
                lastExpense.OccurredOn.ToString(DateTimeFormats.Date, CultureInfo.InvariantCulture),
                lastExpense.CreatedOnUtc.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            return new ExpenseListResponse(expenses[..^1], cursor);
        }
    }
}
