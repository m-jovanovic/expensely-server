using System;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public sealed record TransactionResponse(
        string Id,
        string Description,
        int Category,
        decimal Amount,
        int Currency,
        DateTime OccurredOn,
        int TransactionType);
}
