using Expensely.Authorization;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Expensely.WebApp.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

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

            AddAuthorizationServices(services);
        }

        private static void AddAuthorizationServices(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(AuthorizationAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<ITransient>(), false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());

        private static void AddAuthorizationPolicyProvider(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(AuthorizationAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<IAuthorizationPolicyProvider>(), false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

        private static void AddAuthorizationHandler(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(AuthorizationAssembly.Assembly)
                    .AddClasses(filter => filter.AssignableTo<IAuthorizationHandler>(), false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
    }
}
