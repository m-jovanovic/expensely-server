using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Queries.Transactions.GetTransactions;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Messaging;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactions
{
    /// <summary>
    /// Represents the <see cref="GetTransactionsQuery"/> handler.
    /// </summary>
    internal sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, Maybe<TransactionListResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IDbConnectionProvider _dbConnectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionsQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        public GetTransactionsQueryHandler(IUserInformationProvider userInformationProvider, IDbConnectionProvider dbConnectionProvider)
        {
            _userInformationProvider = userInformationProvider;
            _dbConnectionProvider = dbConnectionProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionListResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionListResponse>.None;
            }

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"
                SELECT TOP(@Limit) Id, Amount, Currency, OccurredOn, CreatedOnUtc
                FROM [Transaction]
                WHERE
                    UserId = @UserId AND
                    (OccurredOn < @OccurredOn OR (OccurredOn = @OccurredOn AND CreatedOnUtc <= @CreatedOnUtc))
                ORDER BY OccurredOn DESC, CreatedOnUtc DESC";

            TransactionResponse[] transactions = (
                await dbConnection.QueryAsync<TransactionResponse>(
                    sql,
                    new
                    {
                        request.UserId,
                        request.OccurredOn,
                        request.CreatedOnUtc,
                        request.Limit
                    })).ToArray();

            if (transactions.Length < request.Limit)
            {
                return new TransactionListResponse(transactions);
            }

            TransactionResponse lastTransaction = transactions[^1];

            string cursor = Cursor.Create(
                lastTransaction.OccurredOn.ToString(DateTimeFormats.Date, CultureInfo.InvariantCulture),
                lastTransaction.CreatedOnUtc.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            return new TransactionListResponse(transactions[..^1], cursor);
        }
    }
}
