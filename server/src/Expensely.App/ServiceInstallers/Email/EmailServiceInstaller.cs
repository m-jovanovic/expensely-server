using Expensely.App.Abstractions;
using Expensely.App.Extensions;
using Expensely.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Email
{
    /// <summary>
    /// Represents the email services installer.
    /// </summary>
    public sealed class EmailServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<EmailSettingsSetup>();

            services.ConfigureOptions<AlertSettingsSetup>();

            services.AddTransientAsMatchingInterface(NotificationAssembly.Assembly);
        }
    }
}
