using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Abstractions.Clock;
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
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="dateTime">The date and time.</param>
        public EventPublisher(IDateTime dateTime) => _dateTime = dateTime;

        /// <inheritdoc />
        public async Task PublishAsync(IEvent @event, IDbTransaction transaction = null) =>
            await PublishAsync(new[] { @event }, transaction);

        /// <inheritdoc />
        public Task PublishAsync(IEnumerable<IEvent> events, IDbTransaction transaction = null) =>
            throw new NotImplementedException();

        /// <summary>
        /// Converts the specified <see cref="IEvent"/> instance into a <see cref="Message"/> instance.
        /// </summary>
        /// <param name="event">The event instance to be converted.</param>
        /// <returns>The message instance.</returns>
        private Message ConvertEventToMessage(IEvent @event) =>
            new(
                @event.GetType().Name,
                JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                _dateTime.UtcNow);
    }
}
