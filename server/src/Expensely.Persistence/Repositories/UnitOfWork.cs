using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the unit of work.
    /// </summary>
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="messageRepository">The message repository.</param>
        public UnitOfWork(IAsyncDocumentSession session, IMessageRepository messageRepository)
        {
            _session = session;
            _messageRepository = messageRepository;
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (string documentId in _session.Advanced.WhatChanged().Keys)
            {
                object document = await _session.LoadAsync<object>(documentId, cancellationToken);

                if (document is not AggregateRoot aggregateRoot || !aggregateRoot.Events.Any())
                {
                    continue;
                }

                foreach (IEvent @event in aggregateRoot.Events)
                {
                    await _session.StoreAsync(new Message(@event), cancellationToken);
                }
            }

            await _session.SaveChangesAsync(cancellationToken);
        }
    }
}
