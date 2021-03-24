using System.Linq;
using System.Reflection;
using Expensely.App.Abstractions;
using Expensely.Common.Primitives.ServiceLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Expensely.App.Extensions
{
    /// <summary>
    /// Contains extensions methods for the service collection class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Install all of the services from the specified assembly.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly to install services from.</param>
        public static void InstallServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var serviceInstallers = ServiceInstallerFactory.GetServiceInstallersFromAssembly(assembly).ToList();

            serviceInstallers.ForEach(x => x.InstallServices(services));
        }

        /// <summary>
        /// Registers the transient services from the specified assembly as matching interface.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly to scan for transient services.</param>
        public static void AddTransientAsMatchingInterface(this IServiceCollection services, Assembly assembly) =>
            services.Scan(scan =>
                scan.FromAssemblies(assembly)
                    .AddClasses(filter => filter.AssignableTo<ITransient>(), false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());

        /// <summary>
        /// Registers the transient services from the specified assembly as matching interface.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly to scan for transient services.</param>
        public static void AddScopedAsMatchingInterface(this IServiceCollection services, Assembly assembly) =>
            services.Scan(scan =>
                scan.FromAssemblies(assembly)
                    .AddClasses(filter => filter.AssignableTo<IScoped>(), false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                    .AsMatchingInterface()
                    .WithTransientLifetime());
    }
}
