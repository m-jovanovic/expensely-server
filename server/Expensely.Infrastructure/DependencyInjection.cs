using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Common;
using Expensely.Infrastructure.Common;
using Expensely.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Infrastructure
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, MachineDateTime>();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

            return services;
        }
    }
}
