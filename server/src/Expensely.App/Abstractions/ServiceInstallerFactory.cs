using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Expensely.App.Abstractions
{
    /// <summary>
    /// Represents the service installer factory.
    /// </summary>
    public static class ServiceInstallerFactory
    {
        /// <summary>
        /// Gets all of the service installers from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan for installers.</param>
        /// <returns>The list of found service installer instances.</returns>
        public static IEnumerable<IServiceInstaller> GetServiceInstallersFromAssembly(Assembly assembly)
        {
            Type serviceInstallerType = typeof(IServiceInstaller);

            IEnumerable<IServiceInstaller> serviceInstallers = assembly.DefinedTypes
                .Where(x => serviceInstallerType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as IServiceInstaller);

            return serviceInstallers;
        }
    }
}
