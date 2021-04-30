using Expensely.App.Abstractions;
using Expensely.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Logging
{
    /// <summary>
    /// Represents the logging services installer.
    /// </summary>
    public class LoggingServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<LoggingOptionsSetup>();

            services.AddTransient<ILoggerConfigurator, LoggerConfigurator>();
        }
    }
}
