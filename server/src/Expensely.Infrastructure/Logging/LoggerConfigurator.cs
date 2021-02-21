using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Serilog;
using Serilog.Events;

namespace Expensely.Infrastructure.Logging
{
    /// <summary>
    /// Represents the logger configurator.
    /// </summary>
    public sealed class LoggerConfigurator : ILoggerConfigurator
    {
        private const string SourceContextKey = "SourceContext";
        private const string AppSourceContext = "Expensely";
        private readonly IConfiguration _configuration;
        private readonly IDocumentStore _documentStore;
        private readonly LoggingSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerConfigurator"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="documentStore">The document store.</param>
        /// <param name="loggingSettingsOptions">The logging settings options.</param>
        public LoggerConfigurator(
            IConfiguration configuration,
            IDocumentStore documentStore,
            IOptions<LoggingSettings> loggingSettingsOptions)
        {
            _configuration = configuration;
            _documentStore = documentStore;
            _settings = loggingSettingsOptions.Value;
        }

        /// <inheritdoc />
        public void Configure() =>
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .WriteTo.RavenDB(_documentStore, logExpirationCallback: LogExpirationCallback)
                .CreateLogger();

        private TimeSpan LogExpirationCallback(LogEvent logEvent)
        {
            TimeSpan documentExpiration = logEvent.Level switch
            {
                LogEventLevel.Error => _settings.ErrorExpirationInDays,
                LogEventLevel.Fatal => _settings.ErrorExpirationInDays,
                _ => _settings.ExpirationInDays
            };

            if (!logEvent.Properties.TryGetValue(SourceContextKey, out LogEventPropertyValue propertyValue))
            {
                return documentExpiration;
            }

            if (propertyValue is not ScalarValue scalarValue)
            {
                return documentExpiration;
            }

            if (!scalarValue.Value.ToString()!.StartsWith(AppSourceContext, StringComparison.Ordinal))
            {
                return documentExpiration;
            }

            // If the log event came from inside our system, there is no expiration as we consider those logs valuable.
            return Timeout.InfiniteTimeSpan;
        }
    }
}
