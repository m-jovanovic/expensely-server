using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Expensely.App.Middleware
{
    /// <summary>
    /// Represents the log context enrichment middleware.
    /// </summary>
    public sealed class LogContextEnrichmentMiddleware : IMiddleware
    {
        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (LogContext.Push(GetEnrichers(context)))
            {
                await next(context);
            }
        }

        /// <summary>
        /// Gets the array of enrichers for the current request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The array of enrichers for the current request.</returns>
        private static ILogEventEnricher[] GetEnrichers(HttpContext context) =>
            new ILogEventEnricher[]
            {
                new PropertyEnricher("IPAddress", context.Connection.RemoteIpAddress),
                new PropertyEnricher("RequestHost", context.Request.Host),
                new PropertyEnricher("RequestPathBase", context.Request.PathBase),
                new PropertyEnricher("RequestQueryParams", context.Request.QueryString)
            };
    }
}
