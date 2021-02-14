using Expensely.Api.Abstractions;
using Expensely.Presentation.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Installers.Presentation
{
    /// <summary>
    /// Represents the presentation installer.
    /// </summary>
    internal sealed class PresentationInstaller : IInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<ApiBehaviorConfigurator>();

            services.AddControllers().AddApplicationPart(PresentationAssembly.Assembly);

            services.AddHttpContextAccessor();
        }
    }
}
