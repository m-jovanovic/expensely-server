using Expensely.WebApp.ModelBinders.Ulid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Expensely.WebApp.ServiceInstallers.Api
{
    /// <summary>
    /// Represents the <see cref="MvcOptions"/> setup.
    /// </summary>
    public sealed class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options) => options.ModelBinderProviders.Insert(0, new UlidModelBinderProvider());
    }
}
