using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the transaction repository.
    /// </summary>
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public TransactionRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<Transaction>> GetByIdAsync(string transactionId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<Transaction>(transactionId, cancellationToken);

        /// <inheritdoc />
        public async Task<Maybe<Transaction>> GetByIdWithUserAsync(string transactionId, CancellationToken cancellationToken = default) =>
            await _session.Include<Transaction>(x => x.UserId).LoadAsync<Transaction>(transactionId, cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(transaction, cancellationToken);

        /// <inheritdoc />
        public void Remove(Transaction transaction) => _session.Delete(transaction);
    }
}
