using System;
using System.Collections.Generic;
using Expensely.Application.Contracts.Transactions;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Transactions
{
    /// <summary>
    /// Represents the query for getting the monthly expenses per category.
    /// </summary>
    public sealed class GetCurrentMonthExpensesPerCategoryQuery : IQuery<IEnumerable<ExpensePerCategoryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthExpensesPerCategoryQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public GetCurrentMonthExpensesPerCategoryQuery(Ulid userId, int currency, DateTime utcNow)
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
