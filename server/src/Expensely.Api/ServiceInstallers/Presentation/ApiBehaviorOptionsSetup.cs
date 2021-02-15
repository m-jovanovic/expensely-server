using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Expensely.Api.ServiceInstallers.Presentation
{
    /// <summary>
    /// Represents the <see cref="ApiBehaviorOptions"/> setup.
    /// </summary>
    public sealed class ApiBehaviorOptionsSetup : IConfigureOptions<ApiBehaviorOptions>
    {
        /// <inheritdoc />
        public void Configure(ApiBehaviorOptions options) => options.SuppressModelStateInvalidFilter = true;
    }
}
