using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction repository interface.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Gets the transaction with the specified identifier, if one exists.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the transaction with the specified identifier.</returns>
        Task<Maybe<Transaction>> GetByIdAsync(Ulid transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the transaction with the specified identifier along with the user who created it, if one exists.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the transaction with the specified identifier.</returns>
        Task<Maybe<Transaction>> GetByIdWithUserAsync(Ulid transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified transaction to the repository.
        /// </summary>
        /// <param name="transaction">The transaction to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified transaction from the repository.
        /// </summary>
        /// <param name="transaction">The transaction to be removed.</param>
        void Remove(Transaction transaction);
    }
}
