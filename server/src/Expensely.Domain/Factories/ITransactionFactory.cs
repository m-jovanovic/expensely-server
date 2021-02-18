using System;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Domain.Factories
{
    /// <summary>
    /// Represents the transaction factory interface.
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Creates a new transaction based on the provided transaction information.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="description">The description.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        /// <param name="transactionTypeId">The transaction type identifier.</param>
        /// <returns>The result of the transaction creation process containing the transaction or an error.</returns>
        public Result<Transaction> Create(
            User user,
            string description,
            int categoryId,
            decimal amount,
            int currencyId,
            DateTime occurredOn,
            int transactionTypeId);
    }
}
