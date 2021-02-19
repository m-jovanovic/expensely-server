using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Expensely.Domain.Primitives;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.BackgroundTasks.MessageProcessing.Factories
{
    /// <summary>
    /// Represents the event handler factory.
    /// </summary>
    public sealed class EventHandlerFactory : IEventHandlerFactory
    {
        private const string HandleMethodName = "Handle";
        private static readonly Type EventHandlerGenericType = typeof(IEventHandler<>).GetGenericTypeDefinition();
        private static readonly ConcurrentDictionary<Type, Type> EventHandlerInterfaceDefinitionsDictionary = new();
        private static readonly ConcurrentDictionary<(Type HandlerType, Type[] HandleMethodArgumentTypes), MethodInfo>
            EventHandlerHandleMethodDictionary = new();

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
        public MethodInfo GetHandleMethod(Type handlerType, Type[] types)
        {
            if (handlerType is null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }

            if (types is null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            return EventHandlerHandleMethodDictionary.GetOrAdd(
                (handlerType, types),
                x => x.HandlerType.GetMethod(HandleMethodName, x.HandleMethodArgumentTypes));
        }
    }
}
