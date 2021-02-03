using Expensely.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Expensely.Api.Extensions
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

        /// <summary>
        /// Configures the Swagger and SwaggerUI middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder builder)
        {
            builder.UseSwagger();

            builder.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensely API");

                swaggerUiOptions.DisplayRequestDuration();
            });

            return builder;
        }
    }
}
