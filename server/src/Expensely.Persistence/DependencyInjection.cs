using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Expensely.Application.Queries.Processors.Abstractions;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Repositories;
using Expensely.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

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
            // TODO: Fix the document store registration.
            services.AddSingleton<IDocumentStore>(serviceProvider =>
            {
                // TODO: Wire up all sorts of conventions.
                var documentStore = new DocumentStore
                {
                    Certificate = new X509Certificate2(configuration["RavenDB:CertificatePath"]),
                    Database = configuration["RavenDB:Database"],
                    Urls = configuration["RavenDB:Urls"].Split(',')
                };

                documentStore.OnBeforeStore += (sender, args) =>
                {
                    if (args.Entity is IAuditableEntity auditableEntity)
                    {
                        // TODO: Set the values.
                    }
                };

                documentStore.Initialize();

                // TODO: Extract database creation out of here.
                var getDatabaseRecordOperation = new GetDatabaseRecordOperation(documentStore.Database);

                DatabaseRecordWithEtag databaseRecord = documentStore.Maintenance.Server.Send(getDatabaseRecordOperation);

                if (databaseRecord is null)
                {
                    var createDatabaseOperation = new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database));

                    documentStore.Maintenance.Server.Send(createDatabaseOperation);
                }

                // TODO: Extract index creation out of here.
                // IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), documentStore);
                return documentStore;
            });

            services.AddScoped(factory => factory.GetRequiredService<IDocumentStore>().OpenAsyncSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<IIncomeRepository, IncomeRepository>();

            services.AddScoped<IBudgetRepository, BudgetRepository>();

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
