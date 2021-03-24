using Expensely.BackgroundTasks.MessageProcessing.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the <see cref="MessageProcessingJobSettings"/> setup.
    /// </summary>
    public sealed class MessageProcessingJobSettingsSetup : IConfigureOptions<MessageProcessingJobSettings>
    {
        private const string ConfigurationSectionName = "Messaging:MessageProcessingJob";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJobSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageProcessingJobSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(MessageProcessingJobSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
