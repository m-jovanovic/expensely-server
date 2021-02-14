using System;
using System.Linq;
using System.Reflection;
using Expensely.Api.Abstractions;

namespace Expensely.Api.Infrastructure
{
    /// <summary>
    /// Represents the installer factory.
    /// </summary>
    public static class InstallerFactory
    {
        /// <summary>
        /// Gets all of the installers from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan for installers.</param>
        /// <returns>The array of found installer instances.</returns>
        public static IInstaller[] GetInstallersFromAssembly(Assembly assembly)
        {
            Type installerType = typeof(IInstaller);

            IInstaller[] installers = assembly.DefinedTypes
                .Where(x => installerType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as IInstaller)
                .ToArray();

            return installers;
        }
    }
}
