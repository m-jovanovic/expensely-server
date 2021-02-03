using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Serilog;
using Serilog.Events;

namespace Expensely.Api.Infrastructure
{
    /// <summary>
    /// Represents the logger configurator.
    /// </summary>
    public static class LoggerConfigurator
    {
        private const string SourceContextKey = "SourceContext";
        private const string InternalSourceContext = "Expensely";
        private static readonly TimeSpan DefaultExpirationInDays = TimeSpan.FromDays(5);
        private static readonly TimeSpan DefaultErrorExpirationInDays = TimeSpan.FromDays(7);

        /// <summary>
        /// Configures the logger using the specified configuration and the specified document store.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="documentStore">The document store.</param>
        public static void Configure(IConfiguration configuration, IDocumentStore documentStore) =>
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.RavenDB(documentStore, logExpirationCallback: LogExpirationCallback)
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
