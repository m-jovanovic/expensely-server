using Expensely.App.ModelBinders.Ulid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Expensely.App.ServiceInstallers.Api
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
