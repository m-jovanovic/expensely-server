using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Application.Abstractions.Messaging
{
    /// <summary>
    /// Represents the event publisher interface.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="event">The events.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="events">The events to be published.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task PublishAsync(IEnumerable<IEvent> @events, CancellationToken cancellationToken = default);
    }
}
