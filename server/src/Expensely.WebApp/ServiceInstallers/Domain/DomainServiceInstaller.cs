using Expensely.Domain.Modules.Transactions;
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
            services.AddTransient<ITransactionDetailsValidator, TransactionDetailsValidator>();

            services.AddTransient<ITransactionFactory, TransactionFactory>();
        }
    }
}
