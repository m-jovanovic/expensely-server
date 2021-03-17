using Expensely.Infrastructure;
using Expensely.WebApp.Abstractions;
using Expensely.WebApp.Extensions;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransientAsMatchingInterface(InfrastructureAssembly.Assembly);

            services.AddScopedAsMatchingInterface(InfrastructureAssembly.Assembly);
        }
    }
}
