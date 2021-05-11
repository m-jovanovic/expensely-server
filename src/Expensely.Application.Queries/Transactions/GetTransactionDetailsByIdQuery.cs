using System;
using Expensely.Application.Contracts.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Transactions
{
    /// <summary>
    /// Represents the query for getting transaction details by identifier.
    /// </summary>
    public sealed class GetTransactionDetailsByIdQuery : IQuery<Maybe<TransactionDetailsResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionDetailsByIdQuery"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        public GetTransactionDetailsByIdQuery(Ulid transactionId) => TransactionId = transactionId;

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        public Ulid TransactionId { get; }
    }
}
