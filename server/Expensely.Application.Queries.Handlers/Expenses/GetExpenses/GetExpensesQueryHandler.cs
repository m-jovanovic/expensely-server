using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Messaging;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Expenses.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, Maybe<ExpenseListResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IDbConnectionProvider _dbConnectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        public GetExpensesQueryHandler(IUserInformationProvider userInformationProvider, IDbConnectionProvider dbConnectionProvider)
        {
            _userInformationProvider = userInformationProvider;
            _dbConnectionProvider = dbConnectionProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<ExpenseListResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Maybe<ExpenseListResponse>.None;
            }

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"
                SELECT TOP(@Limit) Id, Amount, Currency, OccurredOn, CreatedOnUtc
                FROM [Transaction]
                WHERE
                    TransactionType = @TransactionType AND
                    UserId = @UserId AND
                    (OccurredOn < @OccurredOn OR (OccurredOn = @OccurredOn AND CreatedOnUtc <= @CreatedOnUtc))
                ORDER BY OccurredOn DESC, CreatedOnUtc DESC";

            ExpenseResponse[] expenses = (
                await dbConnection.QueryAsync<ExpenseResponse>(
                    sql,
                    new
                    {
                        TransactionType = (int)TransactionType.Expense,
                        request.UserId,
                        request.OccurredOn,
                        request.CreatedOnUtc,
                        request.Limit
                    })).ToArray();

            if (expenses.Length < request.Limit)
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
