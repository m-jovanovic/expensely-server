using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Expensely.WebApp.ServiceInstallers.Domain
{
    /// <summary>
    /// Represents the domain service installer.
    /// </summary>
    public sealed class DomainServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblies()
                    .AddClasses(filter => filter.AssignableTo<ITransient>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());

            services.Scan(scan =>
                scan.FromAssemblies()
                    .AddClasses(filter => filter.AssignableTo<IScoped>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());
        }
    }
}
