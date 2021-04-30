using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Expensely.App.ServiceInstallers.Documentation
{
    /// <summary>
    /// Represents the <see cref="SwaggerUIOptions"/> setup.
    /// </summary>
    public sealed class SwaggerUIOptionsSetup : IConfigureOptions<SwaggerUIOptions>
    {
        /// <inheritdoc />
        public void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensely API");

            options.DisplayRequestDuration();
        }
    }
}
