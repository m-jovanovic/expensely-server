using Expensely.Domain;
using Expensely.WebApp.Abstractions;
using Expensely.WebApp.Extensions;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransientServices(DomainAssembly.Assembly);

            services.AddScopedServices(DomainAssembly.Assembly);
        }
    }
}
