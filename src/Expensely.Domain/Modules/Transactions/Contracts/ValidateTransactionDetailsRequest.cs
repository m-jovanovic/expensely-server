using System;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Modules.Transactions.Contracts
{
    /// <summary>
    /// Represents the request for validating transaction details.
    /// </summary>
    public sealed record ValidateTransactionDetailsRequest(
        User User,
        string Description,
        int Category,
        decimal Amount,
        int Currency,
        DateTime OccurredOn,
        int TransactionType);
}
