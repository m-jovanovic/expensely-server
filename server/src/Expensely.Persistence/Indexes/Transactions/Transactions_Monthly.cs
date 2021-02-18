using System.Linq;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Transactions
{
    /// <summary>
    /// Represents the map-reduce index on the transactions collection and calculates the monthly transactions.
    /// </summary>
    public sealed class Transactions_Monthly : AbstractIndexCreationTask<Transaction, Transactions_Monthly.Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions_Monthly"/> class.
        /// </summary>
        public Transactions_Monthly()
        {
            Map = transactions =>
                from transaction in transactions
                select new Result
                {
                    UserId = transaction.UserId,
                    Year = transaction.OccurredOn.Year,
                    Month = transaction.OccurredOn.Month,
                    TransactionType = transaction.TransactionType.Value,
                    Currency = transaction.Money.Currency.Value,
                    Amount = transaction.Money.Amount
                };

            Reduce = results =>
                from result in results
                group result by new { result.UserId, result.Year, result.Month, result.TransactionType, result.Currency }
                into grouped
                select new Result
                {
                    UserId = grouped.Key.UserId,
                    Year = grouped.Key.Year,
                    Month = grouped.Key.Month,
                    TransactionType = grouped.Key.TransactionType,
                    Currency = grouped.Key.Currency,
                    Amount = grouped.Sum(x => x.Amount)
                };
        }

        /// <summary>
        /// Represents the <see cref="Transactions_Monthly"/> map-reduce result.
        /// </summary>
        public sealed class Result
        {
            /// <summary>
            /// Gets the user identifier.
            /// </summary>
            public string UserId { get; init; }

            /// <summary>
            /// Gets the year.
            /// </summary>
            public int Year { get; init; }

            /// <summary>
            /// Gets the month.
            /// </summary>
            public int Month { get; init; }

            /// <summary>
            /// Gets the transaction type.
            /// </summary>
            public int TransactionType { get; init; }

            /// <summary>
            /// Gets the currency.
            /// </summary>
            public int Currency { get; init; }

            /// <summary>
            /// Gets the amount.
            /// </summary>
            public decimal Amount { get; init; }
        }
    }
}
