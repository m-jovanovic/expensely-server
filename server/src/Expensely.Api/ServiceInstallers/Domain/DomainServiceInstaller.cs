using Expensely.Api.Abstractions;
using Expensely.Domain.Factories;
using Expensely.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.ServiceInstallers.Domain
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
