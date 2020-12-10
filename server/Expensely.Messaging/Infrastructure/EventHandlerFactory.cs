using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Expensely.Domain.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Messaging.Infrastructure
{
    /// <summary>
    /// Represents the event handler factory.
    /// </summary>
    internal sealed class EventHandlerFactory
    {
        private const string HandleMethodName = "Handle";

        private static readonly Type EventHandlerGenericType = typeof(IEventHandler<>).GetGenericTypeDefinition();

        private static readonly ConcurrentDictionary<Type, Type> EventHandlerInterfaceDefinitionsDictionary =
            new ConcurrentDictionary<Type, Type>();

        private static readonly ConcurrentDictionary<Type, MethodInfo> EventHandlerHandleMethodDictionary =
            new ConcurrentDictionary<Type, MethodInfo>();

        /// <summary>
        /// Gets the handlers for the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>The collection of event handlers for the specified event.</returns>
        public IEnumerable<object> GetHandlers(IEvent @event, IServiceProvider serviceProvider)
        {
            Type serviceType = EventHandlerInterfaceDefinitionsDictionary.GetOrAdd(@event.GetType(), type =>
            {
                Type eventHandlerInterfaceDefinition = EventHandlerGenericType.MakeGenericType(type);

                return eventHandlerInterfaceDefinition;
            });

            IEnumerable<object> eventHandlers = serviceProvider.GetServices(serviceType);

            return eventHandlers;
        }

        /// <summary>
        /// Gets the handle method for the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>The handle method for the specified handler.</returns>
        public MethodInfo GetHandleMethod(object handler) =>
            EventHandlerHandleMethodDictionary.GetOrAdd(handler.GetType(), type => type.GetMethod(HandleMethodName));
    }
}
