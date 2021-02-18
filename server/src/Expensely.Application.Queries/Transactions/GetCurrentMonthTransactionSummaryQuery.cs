﻿using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Transactions;
using Expensely.Domain.Primitives.Maybe;

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
        /// <param name="userId">The user identifier provider.</param>
        /// <param name="primaryCurrency">The primary currency.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public GetCurrentMonthTransactionSummaryQuery(Guid userId, int primaryCurrency, DateTime utcNow)
        {
            UserId = userId.ToString();
            PrimaryCurrency = primaryCurrency;
            StartOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).Date;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the primary currency.
        /// </summary>
        public int PrimaryCurrency { get; }

        /// <summary>
        /// Gets the start of month date.
        /// </summary>
        public DateTime StartOfMonth { get; }
    }
}
