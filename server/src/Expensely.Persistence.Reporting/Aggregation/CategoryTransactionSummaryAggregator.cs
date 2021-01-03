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
using Expensely.Persistence.Reporting.Specifications.CategoryTransactionSummaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expensely.Persistence.Reporting.Aggregation
{
    /// <summary>
    /// Represents the transaction summary aggregator.
    /// </summary>
    internal sealed class CategoryTransactionSummaryAggregator : ICategoryTransactionSummaryAggregator
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;
        private readonly ILogger<CategoryTransactionSummaryAggregator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTransactionSummaryAggregator"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        /// <param name="logger">The logger.</param>
        public CategoryTransactionSummaryAggregator(
            IReportingDbContext reportingDbContext,
            IDbConnectionProvider dbConnectionProvider,
            IDateTime dateTime,
            ILogger<CategoryTransactionSummaryAggregator> logger)
        {
            _reportingDbContext = reportingDbContext;
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task IncreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default) =>
            await UpdateWithTransactionDetailsAmountAsync(
                transactionDetails,
                (summary, details) => summary.Amount += details.Amount,
                cancellationToken);

        /// <inheritdoc />
        public async Task DecreaseByAmountAsync(TransactionDetails transactionDetails, CancellationToken cancellationToken = default) =>
            await UpdateWithTransactionDetailsAmountAsync(
                transactionDetails,
                (summary, details) => summary.Amount -= details.Amount,
                cancellationToken);

        /// <summary>
        /// Updates the transaction summary amount with the specified transaction details amount, using the specified update action.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <param name="updateAction">The update action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        private async Task UpdateWithTransactionDetailsAmountAsync(
            TransactionDetails transactionDetails,
            Action<CategoryTransactionSummary, TransactionDetails> updateAction,
            CancellationToken cancellationToken = default)
        {
            Maybe<CategoryTransactionSummary> maybeCategoryTransactionSummary = await _reportingDbContext
                .FirstOrDefaultAsync(
                    new CategoryTransactionSummaryByTransactionDetailsSpecification(transactionDetails),
                    cancellationToken);

            if (maybeCategoryTransactionSummary.HasNoValue)
            {
                await InsertForTransactionDetailsAsync(transactionDetails, _dateTime.UtcNow, cancellationToken);

                return;
            }

            try
            {
                updateAction(maybeCategoryTransactionSummary.Value, transactionDetails);

                await _reportingDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                _logger.LogError(
                    dbUpdateConcurrencyException,
                    "Failed to update category transaction summary for {@TransactionDetails}",
                    transactionDetails);

                await AggregateForTransactionDetailsAsync(transactionDetails);
            }
        }

        /// <summary>
        /// Inserts a new category transaction summary based on the specified transaction.
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
            var transactionSummary = new CategoryTransactionSummary
            {
                Id = Guid.NewGuid(),
                UserId = transactionDetails.UserId,
                Year = transactionDetails.OccurredOn.Year,
                Month = transactionDetails.OccurredOn.Month,
                Category = transactionDetails.Category,
                Amount = transactionDetails.Amount,
                Currency = transactionDetails.Currency,
                TransactionType = transactionDetails.TransactionType,
                CreatedOnUtc = utcNow
            };

            _reportingDbContext.Insert(transactionSummary);

            await _reportingDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Aggregates the category transaction summary for the specified transaction.
        /// </summary>
        /// <param name="transactionDetails">The transaction details.</param>
        /// <returns>The completed task.</returns>
        private async Task AggregateForTransactionDetailsAsync(TransactionDetails transactionDetails)
        {
            const string sql = @"
                UPDATE [CategoryTransactionSummary]
                SET ModifiedOnUtc = @ModifiedOnUtc, Amount =
                    (SELECT SUM(t.Amount)
                     FROM [Transaction] t
                     WHERE
                        t.UserId = @UserId AND
                        t.OccurredOn >= @StartOfMonth AND
                        t.OccurredOn <= @EndOfMonth AND
                        t.Category = @Category AND
                        t.Currency = @Currency AND
                        t.TransactionType = @TransactionType
                     GROUP BY t.TransactionType)
                WHERE
                    UserId = @UserId AND
                    Year = @Year AND
                    Month = @Month AND
                    Category = @Category AND
                    Currency = @Currency AND
                    TransactionType = @TransactionType";

            DateTime utcNow = _dateTime.UtcNow;

            DateTime startOfMonth = new DateTime(transactionDetails.OccurredOn.Year, transactionDetails.OccurredOn.Month, 1).Date;

            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date;

            var parameters = new
            {
                transactionDetails.UserId,
                transactionDetails.Category,
                transactionDetails.Currency,
                transactionDetails.TransactionType,
                transactionDetails.OccurredOn.Year,
                transactionDetails.OccurredOn.Month,
                StartOfMonth = startOfMonth,
                EndOfMonth = endOfMonth,
                ModifiedOnUtc = utcNow
            };

            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            await dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
