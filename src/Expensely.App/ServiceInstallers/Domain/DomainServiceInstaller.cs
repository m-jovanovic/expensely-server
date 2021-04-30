using Expensely.App.Abstractions;
using Expensely.App.Extensions;
using Expensely.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Domain
{
    /// <summary>
    /// Represents the domain service installer.
    /// </summary>
    public sealed class DomainServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.AddTransientAsMatchingInterface(DomainAssembly.Assembly);

            services.AddScopedAsMatchingInterface(DomainAssembly.Assembly);
        }
    }
}
