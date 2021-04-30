using Expensely.Persistence.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Persistence
{
    /// <summary>
    /// Represents the <see cref="RavenDbOptions"/> setup.
    /// </summary>
    public sealed class RavenDbOptionsSetup : IConfigureOptions<RavenDbOptions>
    {
        private const string ConfigurationSectionName = "RavenDB";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public RavenDbOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(RavenDbOptions options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
