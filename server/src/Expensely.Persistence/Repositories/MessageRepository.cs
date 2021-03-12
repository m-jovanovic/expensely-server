using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Messages;
using Expensely.Persistence.Indexes.Messages;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the message repository.
    /// </summary>
    public sealed class MessageRepository : IMessageRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public MessageRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<Message>> GetByIdAsync(Ulid messageId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<Message>(messageId.ToString(), cancellationToken);

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<Message>> GetUnprocessedAsync(int numberOfMessages, CancellationToken cancellationToken = default)
        {
            Message[] messages = await _session
                .Query<Message, Messages_Unprocessed>()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreatedOnUtc)
                .Take(numberOfMessages)
                .ToArrayAsync(cancellationToken);

            return messages;
        }

        /// <inheritdoc />
        public async Task AddAsync(Message message, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(message, cancellationToken);

        /// <inheritdoc />
        public void Remove(Message message) => _session.Delete(message);
    }
}
