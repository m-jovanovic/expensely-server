using Expensely.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the <see cref="JwtOptions"/> setup.
    /// </summary>
    public sealed class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string ConfigurationSectionName = "Jwt";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public JwtOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(JwtOptions options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
