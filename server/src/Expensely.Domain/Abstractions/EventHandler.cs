using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Domain.Abstractions
{
    /// <summary>
    /// Represents the abstract event handler.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>, IEventHandler
        where TEvent : IEvent
    {
        /// <inheritdoc />
        public abstract Task Handle(TEvent @event, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public Task Handle(IEvent @event, CancellationToken cancellationToken = default) => Handle((TEvent)@event, cancellationToken);
    }
}
