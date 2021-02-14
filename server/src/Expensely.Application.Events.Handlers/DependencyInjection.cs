using System;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Application.Events.Handlers
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        private const string EventHandlerPostfix = "EventHandler";

        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses(filter =>
                        filter.Where(type => type.Name.EndsWith(EventHandlerPostfix, StringComparison.Ordinal)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
