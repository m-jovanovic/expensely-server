using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Authorization
{
    /// <summary>
    /// Represents the authorization services installer.
    /// </summary>
    public sealed class AuthorizationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services) => services.AddAuthorization();
    }
}
