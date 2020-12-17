using Expensely.Application.Abstractions.Data;
using Expensely.Persistence.Providers;
using Expensely.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Persistence
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
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new ConnectionString { Value = configuration.GetConnectionString("DefaultConnection") });

            services.AddSingleton<IDbConnectionProvider, DbConnectionProvider>();

            return services;
        }
    }
}
