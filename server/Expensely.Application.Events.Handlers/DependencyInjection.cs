using System;
using System.Linq;
using System.Reflection;
using Expensely.Domain.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Application.Events.Handlers
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            Type eventHandlerType = typeof(IEventHandler<>);

            foreach (TypeInfo typeInfo in Assembly.GetExecutingAssembly().DefinedTypes.Where(x => !IsOpenGeneric(x)))
            {
                if (!IsConcrete(typeInfo))
                {
                    continue;
                }

                foreach (Type eventHandlerInterface in
                    typeInfo.GetInterfaces()
                        .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == eventHandlerType))
                {
                    if (eventHandlerInterface.IsAssignableFrom(typeInfo))
                    {
                        services.AddTransient(eventHandlerInterface, typeInfo);
                    }
                }
            }

            return services;
        }

        private static bool IsOpenGeneric(Type type) =>
            type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;

        private static bool IsConcrete(Type type) =>
            !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
    }
}
