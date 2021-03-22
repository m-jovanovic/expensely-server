using Expensely.Notification;
using Expensely.WebApp.Abstractions;
using Expensely.WebApp.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Email
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

            services.ConfigureOptions<NotificationSettingsSetup>();

            services.AddTransientAsMatchingInterface(NotificationAssembly.Assembly);
        }
    }
}
