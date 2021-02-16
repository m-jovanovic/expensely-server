using System.Linq;
using System.Reflection;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.Extensions
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
    }
}
