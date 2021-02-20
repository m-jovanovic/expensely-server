using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Expensely.Domain.Primitives;

namespace Expensely.BackgroundTasks.MessageProcessing.Factories
{
    /// <summary>
    /// Represents the event handler handle method factory.
    /// </summary>
    public sealed class EventHandlerHandleMethodFactory : IEventHandlerHandleMethodFactory
    {
        private const string HandleMethodName = "Handle";
        private static readonly Dictionary<HandleMethodDictionaryKey, MethodInfo> EventHandlersHandleMethodDictionary = new();

        /// <inheritdoc />
        public Task GetHandleMethodTask(object eventHandler, object[] handleMethodArguments)
        {
            if (eventHandler is null)
            {
                throw new ArgumentNullException(nameof(eventHandler));
            }

            if (handleMethodArguments is null)
            {
                throw new ArgumentNullException(nameof(handleMethodArguments));
            }

            Type handlerType = eventHandler.GetType();

            Type[] argumentTypes = handleMethodArguments.Select(x => x.GetType()).ToArray();

            var handleMethodDictionaryKey = new HandleMethodDictionaryKey(handlerType, argumentTypes);

            if (!EventHandlersHandleMethodDictionary.TryGetValue(handleMethodDictionaryKey, out MethodInfo handleMethod))
            {
                handleMethod = handlerType.GetMethod(HandleMethodName, argumentTypes);

                EventHandlersHandleMethodDictionary.Add(handleMethodDictionaryKey, handleMethod);
            }

            var handleMethodTask = (Task)handleMethod!.Invoke(eventHandler, handleMethodArguments);

            return handleMethodTask;
        }

        private sealed class HandleMethodDictionaryKey : ValueObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HandleMethodDictionaryKey"/> class.
            /// </summary>
            /// <param name="handlerType">The handler type.</param>
            /// <param name="argumentTypes">The handle method argument types.</param>
            public HandleMethodDictionaryKey(Type handlerType, Type[] argumentTypes)
            {
                HandlerType = handlerType;
                ArgumentTypes = argumentTypes;
            }

            private Type HandlerType { get; }

            private Type[] ArgumentTypes { get; }

            /// <inheritdoc />
            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return HandlerType;

                foreach (Type argumentType in ArgumentTypes)
                {
                    yield return argumentType;
                }
            }
        }
    }
}
