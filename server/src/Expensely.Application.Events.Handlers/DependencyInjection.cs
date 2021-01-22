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
            foreach (TypeInfo typeInfo in Assembly.GetExecutingAssembly().DefinedTypes.Where(x => !IsOpenGeneric(x) && !IsConcrete(x)))
            {
                bool IsGenericEventHandlerInterface(Type type) =>
                    type.GetTypeInfo().IsGenericType &&
                    type.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>) &&
                    type.GetTypeInfo().IsAssignableFrom(typeInfo);

                foreach (Type interfaceType in typeInfo.GetInterfaces().Where(IsGenericEventHandlerInterface))
                {
                    services.AddTransient(interfaceType, typeInfo);
                }
            }

            return services;
        }

        private static bool IsOpenGeneric(Type type) =>
            type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;

        private static bool IsConcrete(Type type) => !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
    }
}
