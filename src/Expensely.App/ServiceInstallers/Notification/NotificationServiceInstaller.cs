using Expensely.App.Abstractions;
using Expensely.App.Extensions;
using Expensely.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Notification
{
    /// <summary>
    /// Represents the email services installer.
    /// </summary>
    public sealed class NotificationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<EmailOptionsSetup>();

            services.ConfigureOptions<AlertOptionsSetup>();

            services.AddTransientAsMatchingInterface(NotificationAssembly.Assembly);
        }
    }
}
