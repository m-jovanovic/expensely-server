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
        private readonly LoggingOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerConfigurator"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="documentStore">The document store.</param>
        /// <param name="options">The logging options.</param>
        public LoggerConfigurator(
            IConfiguration configuration,
            IDocumentStore documentStore,
            IOptions<LoggingOptions> options)
        {
            _configuration = configuration;
            _documentStore = documentStore;
            _options = options.Value;
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
                LogEventLevel.Error => _options.ErrorExpirationInDays,
                LogEventLevel.Fatal => _options.ErrorExpirationInDays,
                _ => _options.ExpirationInDays
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
