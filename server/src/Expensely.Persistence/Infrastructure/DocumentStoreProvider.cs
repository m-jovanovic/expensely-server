using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Persistence.Serialization;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Expensely.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the document store provider.
    /// </summary>
    internal sealed class DocumentStoreProvider : IDisposable
    {
        private static readonly Dictionary<Type, MethodInfo> SetCreatedOnMethodsDictionary = new();
        private static readonly Dictionary<Type, MethodInfo> SetModifiedOnMethodsDictionary = new();
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentStoreProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="dateTime">The date and time.</param>
        public DocumentStoreProvider(IConfiguration configuration, IDateTime dateTime)
        {
            _dateTime = dateTime;

            DocumentStore = new DocumentStore
            {
                Certificate = new X509Certificate2(configuration["RavenDB:CertificatePath"]),
                Database = configuration["RavenDB:Database"],
                Urls = configuration["RavenDB:Urls"].Split(',')
            };

            DocumentStore.Initialize();

            CreateDatabaseIfItDoesNotExist(DocumentStore);
        }

        /// <summary>
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore { get; }

        /// <inheritdoc />
        public void Dispose() => DocumentStore?.Dispose();

        /// <summary>
        /// Creates the database if it does not exist.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private static void CreateDatabaseIfItDoesNotExist(IDocumentStore documentStore)
        {
            var getDatabaseRecordOperation = new GetDatabaseRecordOperation(documentStore.Database);

            DatabaseRecordWithEtag databaseRecord = documentStore.Maintenance.Server.Send(getDatabaseRecordOperation);

            if (databaseRecord is not null)
            {
                return;
            }

            var createDatabaseOperation = new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database));

            documentStore.Maintenance.Server.Send(createDatabaseOperation);
        }
    }
}
