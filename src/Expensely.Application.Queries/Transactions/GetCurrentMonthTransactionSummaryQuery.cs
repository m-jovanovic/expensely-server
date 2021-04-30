using System;
using Expensely.Application.Contracts.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Transactions
{
    /// <summary>
    /// Represents the query for getting the monthly transaction summary.
    /// </summary>
    public sealed class GetCurrentMonthTransactionSummaryQuery : IQuery<Maybe<TransactionSummaryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthTransactionSummaryQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public GetCurrentMonthTransactionSummaryQuery(Ulid userId, int currency, DateTime utcNow)
        {
            UserId = userId;
            Currency = currency;
            StartOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).Date;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; }

        /// <summary>
        /// Gets the start of month date.
        /// </summary>
        public DateTime StartOfMonth { get; }
    }
}
