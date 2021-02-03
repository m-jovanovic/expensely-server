using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Expensely.Api.Middleware
{
    /// <summary>
    /// Represents the log context enrichment middleware.
    /// </summary>
    public class LogContextEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogContextEnrichmentMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate pointing to the next middleware in the chain.</param>
        public LogContextEnrichmentMiddleware(RequestDelegate next) => _next = next;

        /// <summary>
        /// Invokes the middleware with the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns>The task that can be awaited by the next middleware.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (LogContext.Push(GetEnrichers(httpContext)))
            {
                await _next(httpContext);
            }
        }

        /// <summary>
        /// Gets the array of enrichers for the current request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The array of enrichers for the current request.</returns>
        private static ILogEventEnricher[] GetEnrichers(HttpContext httpContext) =>
            new ILogEventEnricher[]
            {
                new PropertyEnricher("IPAddress", httpContext.Connection.RemoteIpAddress),
                new PropertyEnricher("RequestHost", httpContext.Request.Host),
                new PropertyEnricher("RequestBasePath", httpContext.Request.Path),
                new PropertyEnricher("RequestQueryParams", httpContext.Request.QueryString)
            };
    }
}
