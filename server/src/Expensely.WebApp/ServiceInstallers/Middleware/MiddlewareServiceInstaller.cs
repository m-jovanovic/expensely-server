using Expensely.Presentation.Api;
using Expensely.WebApp.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Expensely.WebApp.ServiceInstallers.Middleware
{
    /// <summary>
    /// Represents the middleware services installer.
    /// </summary>
    public sealed class MiddlewareServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(PresentationAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<IMiddleware>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsSelf()
                    .WithTransientLifetime());
    }
}
