using Expensely.Presentation.Api;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Api
{
    /// <summary>
    /// Represents the presentation services installer.
    /// </summary>
    public sealed class ApiServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<ApiBehaviorOptionsSetup>();

            services.AddControllers().AddApplicationPart(PresentationAssembly.Assembly);

            services.AddHttpContextAccessor();

            services.AddOptions();
        }
    }
}
