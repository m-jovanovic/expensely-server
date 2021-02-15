using Expensely.Api.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.ServiceInstallers.Documentation
{
    /// <summary>
    /// Represents the documentation services installer.
    /// </summary>
    public sealed class DocumentationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<SwaggerGenOptionsSetup>();

            services.ConfigureOptions<SwaggerUIOptionsSetup>();

            services.AddSwaggerGen();
        }
    }
}
