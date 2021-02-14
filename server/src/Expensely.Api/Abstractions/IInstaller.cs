using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Abstractions
{
    /// <summary>
    /// Represents the service installer interface.
    /// </summary>
    public interface IInstaller
    {
        /// <summary>
        /// Installs the required services using the specified service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        void InstallServices(IServiceCollection services);
    }
}
