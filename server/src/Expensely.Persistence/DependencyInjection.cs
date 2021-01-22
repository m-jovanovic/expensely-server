using Expensely.Domain.Repositories;
using Expensely.Persistence.Repositories;
using Expensely.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace Expensely.Persistence
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new ConnectionString { Value = configuration.GetConnectionString("DefaultConnection") });

            services.AddScoped(factory => factory.GetRequiredService<IDocumentStore>().OpenAsyncSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<IIncomeRepository, IncomeRepository>();

            services.AddScoped<IBudgetRepository, BudgetRepository>();

            return services;
        }
    }
}
