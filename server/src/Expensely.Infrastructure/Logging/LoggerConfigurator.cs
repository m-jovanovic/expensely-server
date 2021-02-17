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
        private readonly LoggingSettings _loggingSettings;

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
            _loggingSettings = loggingSettingsOptions.Value;
        }

        /// <inheritdoc />
        public void Configure() =>
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .WriteTo.RavenDB(_documentStore, logExpirationCallback: LogExpirationCallback)
                .CreateLogger();

        /// <summary>
        /// Returns the timespan indicating when the specified log event should expire.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <remarks>
        /// If the log event came from inside our system, there is not expiration as we consider those logs valuable.
        /// All the log events have some default expiration.
        /// </remarks>
        /// <returns>The timespan indicating when the specified log event should expire.</returns>
        private TimeSpan LogExpirationCallback(LogEvent logEvent)
        {
            TimeSpan documentExpiration = Timeout.InfiniteTimeSpan;

            if (!logEvent.Properties.TryGetValue(SourceContextKey, out LogEventPropertyValue propertyValue))
            {
                return documentExpiration;
            }

            if (propertyValue is not ScalarValue scalarValue)
            {
                return documentExpiration;
            }

            if (scalarValue.Value?.ToString()?.StartsWith(AppSourceContext, StringComparison.Ordinal) ?? false)
            {
                return documentExpiration;
            }

            documentExpiration = logEvent.Level switch
            {
                LogEventLevel.Error => _loggingSettings.ErrorExpirationInDays,
                LogEventLevel.Fatal => _loggingSettings.ErrorExpirationInDays,
                _ => _loggingSettings.ExpirationInDays
            };

            return documentExpiration;
        }
    }
}
