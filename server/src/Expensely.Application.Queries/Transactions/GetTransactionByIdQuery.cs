using Expensely.Application.Contracts.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Transactions
{
    /// <summary>
    /// Represents the query for getting a transaction by identifier.
    /// </summary>
    public sealed record GetTransactionByIdQuery(string TransactionId) : IQuery<Maybe<TransactionResponse>>;
}
