using System;
using Expensely.Api.Abstractions;
using Expensely.Application.Abstractions.Data;
using Expensely.Persistence;
using Expensely.Persistence.Data;
using Expensely.Persistence.Providers;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Scrutor;

namespace Expensely.Api.Installers.Persistence
{
    /// <summary>
    /// Represents the persistence services installer.
    /// </summary>
    internal sealed class PersistenceInstaller : IInstaller
    {
        private const string RepositoryPostfix = "Repository";
        private const string QueryProcessorPostfix = "QueryProcessor";

        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<RavenDbSettingsSetup>();

            services.AddSingleton<DocumentStoreProvider>();

            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<DocumentStoreProvider>().DocumentStore);

            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IDocumentStore>().OpenAsyncSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            AddRepositories(services);

            AddQueryProcessors(services);
        }

        private static void AddRepositories(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(PersistenceAssembly.Assembly)
                    .AddClasses(
                        filter => filter.Where(type => type.Name.EndsWith(RepositoryPostfix, StringComparison.Ordinal)),
                        false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

        private static void AddQueryProcessors(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(PersistenceAssembly.Assembly)
                    .AddClasses(
                        filter => filter.Where(type => type.Name.EndsWith(QueryProcessorPostfix, StringComparison.Ordinal)),
                        false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithScopedLifetime());
    }
}
