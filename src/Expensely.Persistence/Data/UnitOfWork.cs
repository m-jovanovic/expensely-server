using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Expensely.Domain.Primitives;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Data
{
    /// <summary>
    /// Represents the unit of work.
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public UnitOfWork(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await StoreEventsAsMessages(cancellationToken);

            await _session.SaveChangesAsync(cancellationToken);
        }

        private async Task StoreEventsAsMessages(CancellationToken cancellationToken)
        {
            IDictionary<string, DocumentsChanges[]> documentChanges = _session.Advanced.WhatChanged();

            if (!documentChanges.Any())
            {
                return;
            }

            foreach (string documentId in documentChanges.Keys)
            {
                object document = await _session.LoadAsync<object>(documentId, cancellationToken);

                IReadOnlyCollection<IEvent> events;
                if (document is not AggregateRoot aggregateRoot || !(events = aggregateRoot.GetEvents()).Any())
                {
                    continue;
                }

                foreach (IEvent @event in events)
                {
                    await _session.StoreAsync(new Message(@event), cancellationToken);
                }

                aggregateRoot.ClearEvents();
            }
        }
    }
}
