using Expensely.App.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Expensely.App.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring the application builder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the global exception handler middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        /// <summary>
        /// Configures the log context enrichment middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseLogContextEnrichment(this IApplicationBuilder builder)
            => builder.UseMiddleware<LogContextEnrichmentMiddleware>();
    }
}
