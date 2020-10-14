using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Utility;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public GetExpensesQueryHandler(IDbContext dbContext, IUserIdentifierProvider userIdentifierProvider)
        {
            _dbContext = dbContext;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userIdentifierProvider.UserId)
            {
                return Maybe<ExpenseListResponse>.None;
            }

            ExpenseListResponseItem[] expenses = await _dbContext.Set<Expense>()
                .Where(e => e.UserId == _userIdentifierProvider.UserId &&
                            e.OccurredOn < request.OccurredOn ||
                            e.OccurredOn == request.OccurredOn && e.CreatedOnUtc < request.CreatedOnUtc)
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
