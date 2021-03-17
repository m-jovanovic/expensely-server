using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Infrastructure;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Expensely.WebApp.ServiceInstallers.Infrastructure
{
    /// <summary>
    /// Represents the infrastructure services installer.
    /// </summary>
    public class InfrastructureServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblies(InfrastructureAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<ITransient>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());

            services.Scan(scan =>
                scan.FromAssemblies(InfrastructureAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<IScoped>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithScopedLifetime());
        }
    }
}
