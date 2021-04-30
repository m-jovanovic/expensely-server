using Expensely.App.Abstractions;
using Expensely.App.Extensions;
using Expensely.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Infrastructure
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
