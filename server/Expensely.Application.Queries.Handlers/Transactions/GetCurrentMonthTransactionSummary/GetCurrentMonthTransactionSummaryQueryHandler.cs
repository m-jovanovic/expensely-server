using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Queries.Transactions.GetCurrentMonthTransactionSummary;
using Expensely.Common.Messaging;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions.GetCurrentMonthTransactionSummary
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> handler.
    /// </summary>
    internal sealed class GetCurrentMonthTransactionSummaryQueryHandler
        : IQueryHandler<GetCurrentMonthTransactionSummaryQuery, Maybe<TransactionSummaryResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IDbConnectionProvider _dbConnectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthTransactionSummaryQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        public GetCurrentMonthTransactionSummaryQueryHandler(
            IUserInformationProvider userInformationProvider,
            IDbConnectionProvider dbConnectionProvider)
        {
            _userInformationProvider = userInformationProvider;
            _dbConnectionProvider = dbConnectionProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionSummaryResponse>> Handle(
            GetCurrentMonthTransactionSummaryQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId ||
                _userInformationProvider.PrimaryCurrency.HasNoValue ||
                request.PrimaryCurrency != _userInformationProvider.PrimaryCurrency.Value.Value)
            {
                return Maybe<TransactionSummaryResponse>.None;
            }

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"
                SELECT TransactionType, SUM(Amount) AS Amount
                FROM [Transaction]
                WHERE UserId = @UserId AND OccurredOn >= @StartOfMonth AND Currency = @PrimaryCurrency
                GROUP BY TransactionType";

            IEnumerable<TransactionAmountPerType> transactionAmountPerType = await dbConnection.QueryAsync<TransactionAmountPerType>(
                sql,
                new
                {
                    request.UserId,
                    request.StartOfMonth,
                    request.PrimaryCurrency
                });

            var transactionAmountPerTypeDictionary = transactionAmountPerType.ToDictionary(x => x.TransactionType, x => x.Amount);

            Currency currency = Currency.FromValue(request.PrimaryCurrency).Value;

            var response = new TransactionSummaryResponse
            {
                FormattedExpense = FormatAmountForTransactionType(transactionAmountPerTypeDictionary, TransactionType.Expense, currency),
                FormattedIncome = FormatAmountForTransactionType(transactionAmountPerTypeDictionary, TransactionType.Income, currency)
            };

            return response;
        }

        private static string FormatAmountForTransactionType(
            IReadOnlyDictionary<int, decimal> transactionAmountsDictionary,
            TransactionType transactionType,
            Currency currency) =>
            currency.Format(GetAmountForTransactionType(transactionAmountsDictionary, transactionType));

        private static decimal GetAmountForTransactionType(
            IReadOnlyDictionary<int, decimal> transactionAmountsDictionary,
            TransactionType transactionType) =>
            transactionAmountsDictionary.TryGetValue((int)transactionType, out decimal amount)
                ? amount
                : decimal.Zero;
    }
}
