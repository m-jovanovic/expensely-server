using Expensely.Authorization;
using Expensely.WebApp.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Authorization
{
    /// <summary>
    /// Represents the authorization services installer.
    /// </summary>
    public sealed class AuthorizationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.AddAuthorization();

            AddAuthorizationPolicyProvider(services);

            AddAuthorizationHandler(services);
        }

        private static void AddAuthorizationPolicyProvider(IServiceCollection services) =>
            services.Scan(x =>
                x.FromAssemblies(AuthorizationAssembly.Assembly)
                    .AddClasses(x => x.AssignableTo<IAuthorizationPolicyProvider>(), false)
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

        private static void AddAuthorizationHandler(IServiceCollection services) =>
            services.Scan(x => x.FromAssemblies(AuthorizationAssembly.Assembly)
                .AddClasses(x => x.AssignableTo<IAuthorizationHandler>(), false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
    }
}
