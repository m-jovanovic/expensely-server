using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Reporting.Abstractions.Aggregation;
using Expensely.Application.Reporting.Abstractions.Contracts;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Reporting.Transactions;
using Expensely.Persistence.Reporting.Specifications.Transactions;
using Expensely.Persistence.Reporting.Specifications.TransactionSummaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expensely.Persistence.Reporting.Aggregation
{
    /// <summary>
    /// Represents the transaction summary aggregator.
    /// </summary>
    internal sealed class TransactionSummaryAggregator : ITransactionSummaryAggregator
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;
        private readonly ILogger<TransactionSummaryAggregator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionSummaryAggregator"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        /// <param name="logger">The logger.</param>
        public TransactionSummaryAggregator(
            IReportingDbContext reportingDbContext,
            IDbConnectionProvider dbConnectionProvider,
            IDateTime dateTime,
            ILogger<TransactionSummaryAggregator> logger)
        {
            _reportingDbContext = reportingDbContext;
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AggregateAsync(Guid transactionId, int? currency = null, CancellationToken cancellationToken = default)
        {
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionByIdSpecification(transactionId), cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            Transaction transaction = currency.HasValue ? maybeTransaction.Value.WithCurrency(currency.Value) : maybeTransaction.Value;

            bool transactionSummaryExists = await _reportingDbContext
                .AnyAsync(new TransactionSummaryByTransactionSpecification(transaction), cancellationToken);

            if (transactionSummaryExists)
            {
                await AggregateForTransactionAsync(transaction);

                return;
            }

            await InsertForTransactionAsync(transaction, _dateTime.UtcNow, cancellationToken);
        }

        /// <inheritdoc />
        public async Task IncreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default) =>
            await UpdateWithTransactionDetailsAsync(
                transactionDetails,
                (summary, details) => summary.Amount += details.Amount,
                cancellationToken);

        /// <inheritdoc />
        public async Task DecreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default) =>
            await UpdateWithTransactionDetailsAsync(
                transactionDetails,
                (summary, details) => summary.Amount -= details.Amount,
                cancellationToken);

        /// <summary>
        /// Updates the transaction summary for the specified transaction details based on the specified update action.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <param name="updateAction">The update action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        private async Task UpdateWithTransactionDetailsAsync(
            TransactionDetails transactionDetails,
            Action<TransactionSummary, TransactionDetails> updateAction,
            CancellationToken cancellationToken = default)
        {
            Maybe<TransactionSummary> maybeTransactionSummary = await _reportingDbContext
                .FirstOrDefaultAsync(new TransactionSummaryByTransactionDetailsSpecification(transactionDetails), cancellationToken);

            if (maybeTransactionSummary.HasNoValue)
            {
                await InsertForTransactionDetailsAsync(transactionDetails, _dateTime.UtcNow, cancellationToken);

                return;
            }

            try
            {
                updateAction(maybeTransactionSummary.Value, transactionDetails);

                await _reportingDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                _logger.LogError(
                    dbUpdateConcurrencyException,
                    "Failed to update transaction summary amount for user {@UserId}.",
                    transactionDetails.UserId);

                await AggregateForTransactionDetailsAsync(transactionDetails);
            }
        }

        /// <summary>
        /// Inserts a new transaction summary based on the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        private async Task InsertForTransactionAsync(
            Transaction transaction,
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var transactionSummary = new TransactionSummary
            {
                Id = Guid.NewGuid(),
                UserId = transaction.UserId,
                Year = transaction.OccurredOn.Year,
                Month = transaction.OccurredOn.Month,
                Currency = transaction.Currency,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                CreatedOnUtc = utcNow
            };

            _reportingDbContext.Insert(transactionSummary);

            await _reportingDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Inserts a new transaction summary based on the specified transaction.
        /// </summary>
        /// <param name="transactionDetails">The transaction.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        private async Task InsertForTransactionDetailsAsync(
            TransactionDetails transactionDetails,
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var transactionSummary = new TransactionSummary
            {
                Id = Guid.NewGuid(),
                UserId = transactionDetails.UserId,
                Year = transactionDetails.OccurredOn.Year,
                Month = transactionDetails.OccurredOn.Month,
                Currency = transactionDetails.Currency,
                TransactionType = transactionDetails.TransactionType,
                Amount = transactionDetails.Amount,
                CreatedOnUtc = utcNow
            };

            _reportingDbContext.Insert(transactionSummary);

            await _reportingDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Aggregates the transaction summary for the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The completed task.</returns>
        private async Task AggregateForTransactionAsync(Transaction transaction)
        {
            const string sql = @"
                UPDATE [TransactionSummary]
                SET ModifiedOnUtc = @ModifiedOnUtc, Amount =
                    (SELECT SUM(t.Amount)
                     FROM [Transaction] t
                     WHERE
                        t.UserId = @UserId AND
                        t.OccurredOn >= @StartOfMonth AND
                        t.OccurredOn <= @EndOfMonth AND
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

            DateTime startOfMonth = new DateTime(transaction.OccurredOn.Year, transaction.OccurredOn.Month, 1).Date;

            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date;

            var parameters = new
            {
                transaction.UserId,
                transaction.Currency,
                transaction.TransactionType,
                transaction.OccurredOn.Year,
                transaction.OccurredOn.Month,
                StartOfMonth = startOfMonth,
                EndOfMonth = endOfMonth,
                ModifiedOnUtc = utcNow
            };

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            await dbConnection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Aggregates the transaction summary for the specified transaction.
        /// </summary>
        /// <param name="transactionD">The transaction details.</param>
        /// <returns>The completed task.</returns>
        private async Task AggregateForTransactionDetailsAsync(TransactionDetails transactionD)
        {
            const string sql = @"
                UPDATE [TransactionSummary]
                SET ModifiedOnUtc = @ModifiedOnUtc, Amount =
                    (SELECT SUM(t.Amount)
                     FROM [Transaction] t
                     WHERE
                        t.UserId = @UserId AND
                        t.OccurredOn >= @StartOfMonth AND
                        t.OccurredOn <= @EndOfMonth AND
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

            DateTime startOfMonth = new DateTime(transactionD.OccurredOn.Year, transactionD.OccurredOn.Month, 1).Date;

            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date;

            var parameters = new
            {
                transactionD.UserId,
                transactionD.Currency,
                transactionD.TransactionType,
                transactionD.OccurredOn.Year,
                transactionD.OccurredOn.Month,
                StartOfMonth = startOfMonth,
                EndOfMonth = endOfMonth,
                ModifiedOnUtc = utcNow
            };

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            await dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
