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
        /// Configure the custom exception handler middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();

        /// <summary>
        /// Configures the Swagger and SwaggerUI middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder builder)
        {
            builder.UseSwagger();

            builder.UseSwaggerUI(swaggerUiOptions => swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensely API"));

            return builder;
        }
    }
}
