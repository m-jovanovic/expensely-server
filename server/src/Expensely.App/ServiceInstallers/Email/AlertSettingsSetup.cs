using Expensely.Notification.Alert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Email
{
    /// <summary>
    /// Represents the <see cref="AlertSettings"/> setup.
    /// </summary>
    public sealed class AlertSettingsSetup : IConfigureOptions<AlertSettings>
    {
        private const string ConfigurationSectionName = "Alert";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AlertSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(AlertSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
