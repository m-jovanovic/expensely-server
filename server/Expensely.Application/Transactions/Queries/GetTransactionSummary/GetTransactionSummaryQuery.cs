using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Transactions;
using Expensely.Domain.Primitives.Maybe;

namespace Expensely.Application.Transactions.Queries.GetTransactionSummary
{
    /// <summary>
    /// Represents the query for getting the monthly transaction summary.
    /// </summary>
    public sealed class GetTransactionSummaryQuery : IQuery<Maybe<TransactionSummaryResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionSummaryQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier provider.</param>
        /// <param name="primaryCurrency">The primary currency.</param>
        public GetTransactionSummaryQuery(Guid userId, int primaryCurrency)
        {
            UserId = userId;
            PrimaryCurrency = primaryCurrency;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the primary currency.
        /// </summary>
        public int PrimaryCurrency { get; }
    }
}
