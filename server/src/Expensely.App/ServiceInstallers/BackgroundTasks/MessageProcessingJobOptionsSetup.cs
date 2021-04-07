using Expensely.BackgroundTasks.MessageProcessing.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the <see cref="MessageProcessingJobOptions"/> setup.
    /// </summary>
    public sealed class MessageProcessingJobOptionsSetup : IConfigureOptions<MessageProcessingJobOptions>
    {
        private const string ConfigurationSectionName = "Messaging:MessageProcessingJob";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJobOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageProcessingJobOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(MessageProcessingJobOptions options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
