using Expensely.Domain.Modules.Authorization;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Domain
{
    /// <summary>
    /// Represents the domain service installer.
    /// </summary>
    public sealed class DomainServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            // TODO: Use Scrutor.
            services.AddTransient<ITransactionDetailsValidator, TransactionDetailsValidator>();

            services.AddTransient<ITransactionFactory, TransactionFactory>();

            services.AddTransient<IPermissionProvider, PermissionProvider>();

            services.AddScoped<IUserFactory, UserFactory>();
        }
    }
}
