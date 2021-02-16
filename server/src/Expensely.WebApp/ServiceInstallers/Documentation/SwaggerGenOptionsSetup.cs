using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expensely.WebApp.ServiceInstallers.Documentation
{
    /// <summary>
    /// Represents the <see cref="SwaggerGenOptions"/> setup.
    /// </summary>
    public sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
    {
        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Expensely API",
                Description = "Expense tracking application API written in .NET 5.0."
            });

            options.OperationFilter<AuthorizationOperationFilter>();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Description = "JWT Authorization header using the Bearer scheme."
            });
        }
    }
}
