using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions.Entities;
using Newtonsoft.Json;

namespace Expensely.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the event publisher.
    /// </summary>
    public sealed class EventPublisher : IEventPublisher
    {
        private readonly IApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="applicationDbContext">The application database context.</param>
        public EventPublisher(IApplicationDbContext applicationDbContext) => _applicationDbContext = applicationDbContext;

        /// <inheritdoc />
        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default) =>
            await PublishAsync(new[] { @event }, cancellationToken);

        /// <inheritdoc />
        public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            Message[] messages = events.Select(ConvertEventToMessage).ToArray();

            if (!messages.Any())
            {
                return;
            }

            _applicationDbContext.Set<Message>().AddRange(messages);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private static Message ConvertEventToMessage(IEvent @event) =>
            new()
            {
                Id = Guid.NewGuid(),
                Name = @event.GetType().Name,
                Content = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            };
    }
}
