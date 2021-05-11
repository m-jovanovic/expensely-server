using System;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction details response.
    /// </summary>
    public sealed record TransactionDetailsResponse(
        string Id,
        string Description,
        string Category,
        string FormattedAmount,
        DateTime OccurredOn);
}
