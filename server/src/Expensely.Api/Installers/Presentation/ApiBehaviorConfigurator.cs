using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Expensely.Api.Installers.Presentation
{
    /// <summary>
    /// Represents the <see cref="ApiBehaviorOptions"/> configurator.
    /// </summary>
    internal sealed class ApiBehaviorConfigurator : IConfigureOptions<ApiBehaviorOptions>
    {
        /// <inheritdoc />
        public void Configure(ApiBehaviorOptions options) => options.SuppressModelStateInvalidFilter = true;
    }
}
