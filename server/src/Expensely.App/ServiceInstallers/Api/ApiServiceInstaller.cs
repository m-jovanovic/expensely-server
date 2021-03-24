using Expensely.App.Abstractions;
using Expensely.Presentation.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Api
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

            services.ConfigureOptions<MvcOptionsSetup>();

            services.AddControllers().AddApplicationPart(PresentationAssembly.Assembly);

            services.AddHttpContextAccessor();

            services.AddOptions();
        }
    }
}
