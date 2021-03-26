using Expensely.Notification.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Notification
{
    /// <summary>
    /// Represents the <see cref="EmailSettings"/> setup.
    /// </summary>
    public sealed class EmailSettingsSetup : IConfigureOptions<EmailSettings>
    {
        private const string ConfigurationSectionName = "Email";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EmailSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(EmailSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
