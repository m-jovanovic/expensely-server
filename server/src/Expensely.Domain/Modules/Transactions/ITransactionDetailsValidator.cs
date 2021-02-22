using System;
using Expensely.Domain.Modules.Users;
using Expensely.Shared.Primitives.Result;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction details validator interface.
    /// </summary>
    public interface ITransactionDetailsValidator
    {
        /// <summary>
        /// Validates the provided transaction information and returns the result of the validation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="description">The description.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="transactionTypeId">The transaction type identifier.</param>
        /// <returns>The result of the transaction validation process containing the transaction information or an error.</returns>
        Result<TransactionDetails> Validate(
            User user,
            string description,
            int categoryId,
            decimal amount,
            int currencyId,
            DateTime occurredOn,
            int transactionTypeId);
    }
}
