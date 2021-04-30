using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Transactions.Contracts;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction factory interface.
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Creates a new transaction based on the specified <see cref="CreateTransactionRequest"/> instance.
        /// </summary>
        /// <param name="createTransactionRequest">The create transaction request.</param>
        /// <returns>The result of the transaction creation process containing the transaction or an error.</returns>
        public Result<Transaction> Create(CreateTransactionRequest createTransactionRequest);
    }
}
