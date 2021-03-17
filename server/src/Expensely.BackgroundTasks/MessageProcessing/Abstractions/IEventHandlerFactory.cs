using System.Collections.Generic;
using Expensely.Domain.Abstractions;

namespace Expensely.BackgroundTasks.MessageProcessing.Abstractions
{
    /// <summary>
    /// Represents the event handler factory interface.
    /// </summary>
    internal interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets the event handler instances for the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>The collection of event handler instances for the specified event.</returns>
        IEnumerable<IEventHandler> GetHandlers(IEvent @event);
    }
}
