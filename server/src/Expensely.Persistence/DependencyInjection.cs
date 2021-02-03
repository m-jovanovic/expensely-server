using System;
using System.Linq;
using System.Reflection;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Domain.Repositories;
using Expensely.Persistence.Infrastructure;
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
            services.Configure<RavenDbSettings>(configuration.GetSection(RavenDbSettings.SettingsKey));

            services.AddSingleton<DocumentStoreProvider>();

            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<DocumentStoreProvider>().DocumentStore);

            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IDocumentStore>().OpenAsyncSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IBudgetRepository, BudgetRepository>();

            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddQueryProcessors();

            return services;
        }

        private static void AddQueryProcessors(this IServiceCollection services)
        {
            foreach (TypeInfo typeInfo in Assembly.GetExecutingAssembly().DefinedTypes.Where(IsConcrete))
            {
                Type[] interfaces = typeInfo.GetInterfaces();

                static bool IsGenericQueryProcessorInterfaceType(Type type) =>
                    type.GetTypeInfo().IsGenericType && type.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryProcessor<,>);

                if (!interfaces.Any(IsGenericQueryProcessorInterfaceType))
                {
                    continue;
                }

                Type nonGenericQueryProcessorInterface = interfaces.Single(x => !x.IsGenericType);

                services.AddScoped(nonGenericQueryProcessorInterface, typeInfo);
            }
        }

        private static bool IsConcrete(Type type) => !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
    }
}
