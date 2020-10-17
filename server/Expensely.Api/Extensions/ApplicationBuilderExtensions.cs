using Expensely.Api.Middleware;
using Expensely.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        /// <returns>The same application builder.</returns>
        internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();

        /// <summary>
        /// Configures the Swagger and SwaggerUI middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        internal static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder builder)
        {
            builder.UseSwagger();

            builder.UseSwaggerUI(swaggerUiOptions => swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensely API"));

            return builder;
        }

        /// <summary>
        /// Applies all of the pending the migrations.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The same application builder.</returns>
        internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder builder)
        {
            using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();

            using ExpenselyDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ExpenselyDbContext>();

            dbContext.Database.Migrate();

            return builder;
        }
    }
}
