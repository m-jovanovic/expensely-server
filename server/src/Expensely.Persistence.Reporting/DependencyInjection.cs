using System.Reflection;
using Expensely.Application.Abstractions.Aggregation;
using Expensely.Application.Abstractions.Data;
using Expensely.Persistence.Reporting.Aggregation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Persistence.Reporting
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
        public static IServiceCollection AddReportingDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReportingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);

                    optionsBuilder.MigrationsHistoryTable("__ReportingMigrationsHistory");
                }));

            services.AddScoped<IReportingDbContext>(serviceProvider => serviceProvider.GetRequiredService<ReportingDbContext>());

            services.AddScoped<ITransactionSummaryAggregator, TransactionSummaryAggregator>();

            return services;
        }
    }
}
