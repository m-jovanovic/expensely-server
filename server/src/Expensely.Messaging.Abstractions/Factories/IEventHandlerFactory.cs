using System;
using System.Collections.Generic;
using System.Reflection;
using Expensely.Domain.Abstractions.Events;

namespace Expensely.Messaging.Abstractions.Factories
{
    /// <summary>
    /// Represents the event handler factory interface.
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets the handlers for the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>The collection of event handlers for the specified event.</returns>
        IEnumerable<object> GetHandlers(IEvent @event, IServiceProvider serviceProvider);

        /// <summary>
        /// Gets the handle method for the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>The handle method for the specified handler.</returns>
        MethodInfo GetHandleMethod(object handler);
    }
}
