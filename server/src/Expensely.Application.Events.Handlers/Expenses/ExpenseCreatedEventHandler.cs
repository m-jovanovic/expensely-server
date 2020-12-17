using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Data;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Events.Expenses;
using Expensely.Domain.Reporting.Transactions;

namespace Expensely.Application.Events.Handlers.Expenses
{
    /// <summary>
    /// Represents the <see cref="ExpenseCreatedEvent"/> handler.
    /// </summary>
    public sealed class ExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly IReportingDbContext _reportingDbContext;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="reportingDbContext">The reporting database context.</param>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public ExpenseCreatedEventHandler(
            IReportingDbContext reportingDbContext,
            IDbConnectionProvider dbConnectionProvider,
            IDateTime dateTime)
        {
            _reportingDbContext = reportingDbContext;
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task Handle(ExpenseCreatedEvent @event, CancellationToken cancellationToken)
        {
            // TODO: Create specification.
            Maybe<Transaction> maybeTransaction = await _reportingDbContext
                .FirstOrDefaultAsync<Transaction>(x => x.Id == @event.ExpenseId, cancellationToken);

            if (maybeTransaction.HasNoValue)
            {
                return;
            }

            Transaction transaction = maybeTransaction.Value;

            // TODO: Create specification.
            bool transactionSummaryExists = await _reportingDbContext
                .AnyAsync<TransactionSummary>(
                    x =>
                        x.UserId == transaction.UserId &&
                        x.Year == transaction.OccurredOn.Year &&
                        x.Month == transaction.OccurredOn.Month &&
                        x.TransactionType == transaction.TransactionType &&
                        x.Currency == transaction.Currency,
                    cancellationToken);

            if (!transactionSummaryExists)
            {
                var transactionSummary = new TransactionSummary
                {
                    Id = Guid.NewGuid(),
                    UserId = transaction.UserId,
                    Year = transaction.OccurredOn.Year,
                    Month = transaction.OccurredOn.Month,
                    TransactionType = transaction.TransactionType,
                    Currency = transaction.Currency,
                    Amount = transaction.Amount
                };

                _reportingDbContext.Insert(transactionSummary);

                await _reportingDbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                using IDbConnection dbConnection = _dbConnectionProvider.Create();

                const string sql =
                    @"UPDATE [TransactionSummary]
                      SET Amount =
                          (SELECT SUM(t.Amount) FROM [Transaction] t
                           WHERE
                               t.UserId = @UserId AND
                               t.OccurredOn >= @StartOfMonth AND
                               t.Currency = @Currency AND
                               t.TransactionType = @TransactionType
                           GROUP BY t.TransactionType)
                      WHERE UserId = @UserId AND
                            Year = @Year AND
                            Month = @Month AND
                            TransactionType = @TransactionType AND
                            Currency = @Currency";

                DateTime utcNow = _dateTime.UtcNow;

                var param = new
                {
                    transaction.UserId,
                    transaction.Currency,
                    transaction.TransactionType,
                    transaction.OccurredOn.Year,
                    transaction.OccurredOn.Month,
                    StartOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).Date
                };

                await dbConnection.ExecuteAsync(sql, param);
            }
        }
    }
}
