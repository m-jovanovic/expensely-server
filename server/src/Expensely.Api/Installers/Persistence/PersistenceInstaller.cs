using System;
using Expensely.Api.Abstractions;
using Expensely.Persistence;
using Microsoft.Extensions.DependencyInjection;
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

            services.Scan(scan =>
                scan.FromAssemblies(PersistenceAssembly.Assembly)
                    .AddClasses(
                        filter => filter.Where(type =>
                        type.Name.EndsWith(RepositoryPostfix, StringComparison.InvariantCulture) ||
                        type.Name.EndsWith(QueryProcessorPostfix, StringComparison.InvariantCulture)),
                        false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithScopedLifetime());
        }
    }
}
