using Expensely.Notification.Alert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Notification
{
    /// <summary>
    /// Represents the <see cref="AlertOptions"/> setup.
    /// </summary>
    public sealed class AlertOptionsSetup : IConfigureOptions<AlertOptions>
    {
        private const string ConfigurationSectionName = "Alert";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AlertOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(AlertOptions options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
