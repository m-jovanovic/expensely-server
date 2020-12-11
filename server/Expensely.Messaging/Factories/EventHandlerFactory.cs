using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Messaging.Factories
{
    /// <summary>
    /// Represents the event handler factory.
    /// </summary>
    public sealed class EventHandlerFactory : IEventHandlerFactory
    {
        private const string HandleMethodName = "Handle";

        private static readonly Type EventHandlerGenericType = typeof(IEventHandler<>).GetGenericTypeDefinition();

        private static readonly ConcurrentDictionary<Type, Type> EventHandlerInterfaceDefinitionsDictionary =
            new ConcurrentDictionary<Type, Type>();

        private static readonly ConcurrentDictionary<Type, MethodInfo> EventHandlerHandleMethodDictionary =
            new ConcurrentDictionary<Type, MethodInfo>();

        /// <inheritdoc />
        public IEnumerable<object> GetHandlers(IEvent @event, IServiceProvider serviceProvider)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            Type serviceType = EventHandlerInterfaceDefinitionsDictionary.GetOrAdd(
                @event.GetType(),
                eventType => EventHandlerGenericType.MakeGenericType(eventType));

            IEnumerable<object> eventHandlers = serviceProvider.GetServices(serviceType);

            return eventHandlers;
        }

        /// <inheritdoc />
        public MethodInfo GetHandleMethod(object handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return EventHandlerHandleMethodDictionary.GetOrAdd(handler.GetType(), handlerType => handlerType.GetMethod(HandleMethodName));
        }
    }
}
