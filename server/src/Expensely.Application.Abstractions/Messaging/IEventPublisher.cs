using System.Collections.Generic;
using System.Data;
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
        /// <param name="transaction">The database transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task PublishAsync(IEvent @event, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="events">The events to be published.</param>
        /// <param name="transaction">The database transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task PublishAsync(IEnumerable<IEvent> @events, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }
}
