using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Modules.Messages;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the failed message repository.
    /// </summary>
    public sealed class FailedMessageRepository : IFailedMessageRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedMessageRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public FailedMessageRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task AddAsync(FailedMessage failedMessage, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(failedMessage, cancellationToken);
    }
}
