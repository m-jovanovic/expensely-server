using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Expensely.Domain.Abstractions.Events;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;

namespace Expensely.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the event publisher.
    /// </summary>
    public sealed class EventPublisher : IEventPublisher, IScoped
    {
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        public EventPublisher(IMessageRepository messageRepository) => _messageRepository = messageRepository;

        /// <inheritdoc />
        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default) =>
            await _messageRepository.AddAsync(new Message(@event), cancellationToken);

        /// <inheritdoc />
        public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default) =>
            await _messageRepository.AddAsync(events.Select(@event => new Message(@event)), cancellationToken);
    }
}
