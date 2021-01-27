using System;
using System.Linq;
using System.Reflection;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Domain.Repositories;
using Expensely.Persistence.Infrastructure;
using Expensely.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

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
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<DocumentStoreProvider>();

            services.AddScoped(provider => provider.GetRequiredService<DocumentStoreProvider>().DocumentStore.OpenAsyncSession());

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

                static bool IsGenericQueryProcessorInterface(Type type) =>
                    type.GetTypeInfo().IsGenericType && type.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryProcessor<,>);

                if (!interfaces.Any(IsGenericQueryProcessorInterface))
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
