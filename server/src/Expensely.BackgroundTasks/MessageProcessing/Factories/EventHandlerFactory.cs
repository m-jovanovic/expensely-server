using System;
using System.Collections.Generic;
using Expensely.Domain.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.BackgroundTasks.MessageProcessing.Factories
{
    /// <summary>
    /// Represents the event handler factory.
    /// </summary>
    public sealed class EventHandlerFactory : IEventHandlerFactory
    {
        private static readonly Type EventHandlerGenericType = typeof(IEventHandler<>).GetGenericTypeDefinition();
        private static readonly Dictionary<Type, Type> EventHandlersDictionary = new();

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public EventHandlerFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc />
        public IEnumerable<object> GetHandlers(IEvent @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            Type eventType = @event.GetType();

            if (!EventHandlersDictionary.TryGetValue(eventType, out Type handlerType))
            {
                handlerType = EventHandlerGenericType.MakeGenericType(eventType);

                EventHandlersDictionary.Add(eventType, handlerType);
            }

            IEnumerable<object> eventHandlers = _serviceProvider.GetServices(handlerType);

            return eventHandlers;
        }
    }
}
