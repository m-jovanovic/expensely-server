using Expensely.Api.Abstractions;
using Expensely.Presentation.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.ServiceInstallers.Presentation
{
    /// <summary>
    /// Represents the presentation services installer.
    /// </summary>
    public sealed class PresentationServiceInstaller : IServiceInstaller
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
