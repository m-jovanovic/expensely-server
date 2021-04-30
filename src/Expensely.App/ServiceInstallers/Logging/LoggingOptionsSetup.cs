using System;
using Expensely.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Logging
{
    /// <summary>
    /// Represents the <see cref="LoggingOptions"/> setup.
    /// </summary>
    public sealed class LoggingOptionsSetup : IConfigureOptions<LoggingOptions>
    {
        private const string ConfigurationSectionName = "Serilog:RavenDB";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LoggingOptionsSetup(IConfiguration configuration) => _configuration = configuration;

        /// <inheritdoc />
        public void Configure(LoggingOptions options)
        {
            var loggingExpiration = new LoggingExpiration();

            _configuration.GetSection(ConfigurationSectionName).Bind(loggingExpiration);

            options.ExpirationInDays = TimeSpan.FromDays(loggingExpiration.ExpirationInDays);

            options.ErrorExpirationInDays = TimeSpan.FromDays(loggingExpiration.ErrorExpirationInDays);
        }

        private class LoggingExpiration
        {
            public int ExpirationInDays { get; init; }

            public int ErrorExpirationInDays { get; init; }
        }
    }
}
