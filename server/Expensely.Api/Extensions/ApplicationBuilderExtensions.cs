using Expensely.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Expensely.Api.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring the application builder.
    /// </summary>
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the custom exception handler middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The configured application builder.</returns>
        internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
