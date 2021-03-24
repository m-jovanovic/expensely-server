using Expensely.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Persistence
{
    /// <summary>
    /// Represents the <see cref="RavenDbSettings"/> setup.
    /// </summary>
    public sealed class RavenDbSettingsSetup : IConfigureOptions<RavenDbSettings>
    {
        private const string ConfigurationSectionName = "RavenDB";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public RavenDbSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(RavenDbSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
