using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Application.Abstractions.Data;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Persistence.Reporting.Aggregation
{
    /// <summary>
    /// Represents the transaction summary aggregator.
    /// </summary>
    internal sealed class TransactionSummaryAggregator : ITransactionSummaryAggregator
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummaryAggregator"/> class.
        /// </summary>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public TransactionSummaryAggregator(IDbConnectionProvider dbConnectionProvider, IDateTime dateTime)
        {
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task AggregateForTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE [TransactionSummary]
                SET Amount =
                    (SELECT SUM(t.Amount) FROM [Transaction] t
                        WHERE
                           t.UserId = @UserId AND
                           t.OccurredOn >= @StartOfMonth AND
                           t.Currency = @Currency AND
                           t.TransactionType = @TransactionType
                        GROUP BY t.TransactionType)
                    WHERE
                        UserId = @UserId AND
                        Year = @Year AND
                        Month = @Month AND
                        Currency = @Currency AND
                        TransactionType = @TransactionType";

            DateTime utcNow = _dateTime.UtcNow;

            var parameters = new
            {
                transaction.UserId,
                transaction.Currency,
                transaction.TransactionType,
                transaction.OccurredOn.Year,
                transaction.OccurredOn.Month,
                StartOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).Date
            };

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            await dbConnection.ExecuteAsync(sql, parameters);
        }

        /// <inheritdoc />
        public async Task AggregateForTransactionAsync(
            Transaction transaction, int previousCurrency, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE [TransactionSummary]
                    SET Amount =
                        (SELECT SUM(t.Amount) FROM [Transaction] t
                         WHERE
                             t.UserId = @UserId AND
                             t.OccurredOn >= @StartOfMonth AND
                             t.Currency = @Currency AND
                             t.TransactionType = @TransactionType
                         GROUP BY t.TransactionType)
                    WHERE
                        UserId = @UserId AND
                        Year = @Year AND
                        Month = @Month AND
                        Currency = @Currency AND
                        TransactionType = @TransactionType

                IF (@Currency != @PreviousCurrency)
                BEGIN
                    UPDATE [TransactionSummary]
                    SET Amount =
                        (SELECT SUM(t.Amount) FROM [Transaction] t
                         WHERE
                             t.UserId = @UserId AND
                             t.OccurredOn >= @StartOfMonth AND
                             t.Currency = @PreviousCurrency AND
                             t.TransactionType = @TransactionType
                         GROUP BY t.TransactionType)
                    WHERE
                        UserId = @UserId AND
                        Year = @Year AND
                        Month = @Month AND
                        Currency = @PreviousCurrency AND
                        TransactionType = @TransactionType
                END";

            DateTime utcNow = _dateTime.UtcNow;

            var parameters = new
            {
                transaction.UserId,
                transaction.Currency,
                PreviousCurrency = previousCurrency,
                transaction.TransactionType,
                transaction.OccurredOn.Year,
                transaction.OccurredOn.Month,
                StartOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).Date
            };

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            await dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
