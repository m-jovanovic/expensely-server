using Expensely.Infrastructure.Logging;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Logging
{
    /// <summary>
    /// Represents the logging services installer.
    /// </summary>
    public class LoggingServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<LoggingSettingsSetup>();

            services.AddTransient<ILoggerConfigurator, LoggerConfigurator>();
        }
    }
}
