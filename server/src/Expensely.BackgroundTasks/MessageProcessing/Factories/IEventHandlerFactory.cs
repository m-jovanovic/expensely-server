using System.Collections.Generic;
using Expensely.Domain.Primitives;

namespace Expensely.BackgroundTasks.MessageProcessing.Factories
{
    /// <summary>
    /// Represents the event handler factory interface.
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets the event handler instances for the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>The collection of event handler instances for the specified event.</returns>
        IEnumerable<object> GetHandlers(IEvent @event);
    }
}
