using Expensely.Application.Abstractions.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.WebApp.ServiceInstallers.Email
{
    /// <summary>
    /// Represents the <see cref="NotificationSettings"/> setup.
    /// </summary>
    public sealed class NotificationSettingsSetup : IConfigureOptions<NotificationSettings>
    {
        private const string ConfigurationSectionName = "Notification";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public NotificationSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(NotificationSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
