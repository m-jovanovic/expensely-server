using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.BackgroundTasks.MessageProcessing.Abstractions;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.BackgroundTasks.MessageProcessing.Implementations
{
    /// <summary>
    /// Represents the event handler factory.
    /// </summary>
    internal sealed class EventHandlerFactory : IEventHandlerFactory, IScoped
    {
        private static readonly Type EventHandlerGenericType = typeof(IEventHandler<>);
        private static readonly Dictionary<Type, Type> EventHandlersDictionary = new();

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public EventHandlerFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc />
        public IEnumerable<IEventHandler> GetHandlers(IEvent @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            Type eventType = @event.GetType();

            if (!EventHandlersDictionary.TryGetValue(eventType, out Type eventHandlerType))
            {
                eventHandlerType = EventHandlerGenericType.MakeGenericType(eventType);

                EventHandlersDictionary.Add(eventType, eventHandlerType);
            }

            IEnumerable<IEventHandler> eventHandlers = _serviceProvider.GetServices(eventHandlerType).Cast<IEventHandler>();

            return eventHandlers;
        }
    }
}
