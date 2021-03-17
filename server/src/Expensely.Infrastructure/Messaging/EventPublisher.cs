using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Expensely.Domain.Primitives;

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
    }
}
