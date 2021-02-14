using System;
using Expensely.Domain.Repositories;
using Expensely.Persistence.Infrastructure;
using Expensely.Persistence.Repositories;
using Expensely.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Scrutor;

namespace Expensely.Persistence
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        private const string RepositoryPostfix = "Repository";
        private const string QueryProcessorPostfix = "QueryProcessor";

        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RavenDbSettings>(configuration.GetSection(RavenDbSettings.SettingsKey));

            services.AddSingleton<DocumentStoreProvider>();

            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<DocumentStoreProvider>().DocumentStore);

            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IDocumentStore>().OpenAsyncSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses(filter => filter.Where(type =>
                        type.Name.EndsWith(RepositoryPostfix, StringComparison.InvariantCulture) ||
                        type.Name.EndsWith(QueryProcessorPostfix, StringComparison.InvariantCulture)))
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            return services;
        }
    }
}
