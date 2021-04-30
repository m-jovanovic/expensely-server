using System;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Modules.Transactions.Contracts
{
    /// <summary>
    /// Represents the request for creating a new transaction.
    /// </summary>
    public sealed record CreateTransactionRequest(
        User User,
        string Description,
        int Category,
        decimal Amount,
        int Currency,
        DateTime OccurredOn,
        int TransactionType)
    {
        /// <summary>
        /// Creates a new <see cref="ValidateTransactionDetailsRequest"/> from the current request instance.
        /// </summary>
        /// <returns>The new <see cref="ValidateTransactionDetailsRequest"/> instance.</returns>
        internal ValidateTransactionDetailsRequest ToValidateTransactionDetailsRequest() =>
            new(User, Description, Category, Amount, Currency, OccurredOn, TransactionType);
    }
}
