using System;
using System.Threading;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Serilog;
using Serilog.Events;

namespace Expensely.Infrastructure.Logging
{
    /// <summary>
    /// Represents the logger configurator.
    /// </summary>
    public sealed class LoggerConfigurator : ILoggerConfigurator, ITransient
    {
        private const string SourceContextKey = "SourceContext";
        private const string InternalSourceContext = "Expensely";
        private static readonly TimeSpan DefaultExpirationInDays = TimeSpan.FromDays(5);
        private static readonly TimeSpan DefaultErrorExpirationInDays = TimeSpan.FromDays(7);

        private readonly IConfiguration _configuration;
        private readonly IDocumentStore _documentStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerConfigurator"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="documentStore">The document store.</param>
        public LoggerConfigurator(IConfiguration configuration, IDocumentStore documentStore)
        {
            _configuration = configuration;
            _documentStore = documentStore;
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
        private static TimeSpan LogExpirationCallback(LogEvent logEvent)
        {
            if (logEvent.Properties.TryGetValue(SourceContextKey, out LogEventPropertyValue propertyValue) &&
                propertyValue is ScalarValue scalarValue &&
                (scalarValue.Value?.ToString()?.StartsWith(InternalSourceContext, StringComparison.InvariantCulture) ?? false))
            {
                return Timeout.InfiniteTimeSpan;
            }

            return logEvent.Level switch
            {
                LogEventLevel.Error => DefaultErrorExpirationInDays,
                LogEventLevel.Fatal => DefaultErrorExpirationInDays,
                _ => DefaultExpirationInDays
            };
        }
    }
}
