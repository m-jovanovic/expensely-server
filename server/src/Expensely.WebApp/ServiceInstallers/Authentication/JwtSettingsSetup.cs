using Expensely.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.WebApp.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the <see cref="JwtSettings"/> setup.
    /// </summary>
    public sealed class JwtSettingsSetup : IConfigureOptions<JwtSettings>
    {
        private const string ConfigurationSectionName = "Jwt";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSettingsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public JwtSettingsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(JwtSettings options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
