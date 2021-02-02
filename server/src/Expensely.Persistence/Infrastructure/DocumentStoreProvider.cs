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
using Raven.Client.Documents.Operations.Expiration;
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

            // TODO: Make a configuration object for these settings.
            DocumentStore = new DocumentStore
            {
                Certificate = new X509Certificate2(Convert.FromBase64String(configuration["RavenDB:Certificate"])),
                Database = configuration["RavenDB:Database"],
                Urls = configuration["RavenDB:Urls"].Split(',')
            };

            DocumentStore.Conventions.Serialization = new NewtonsoftJsonSerializationConventions
            {
                JsonContractResolver = new CustomContractResolver(DocumentStore.Conventions.Serialization)
            };

            DocumentStore.OnBeforeStore += SetAuditableEntityValues_OnBeforeStore;

            DocumentStore.Initialize();

            CreateDatabaseIfItDoesNotExist(DocumentStore);

            ConfigureExpirationOptions(DocumentStore);

            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), DocumentStore);
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

        /// <summary>
        /// Configures the document expiration options for the database.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private static void ConfigureExpirationOptions(IDocumentStore documentStore)
        {
            var configureExpirationOperation = new ConfigureExpirationOperation(new ExpirationConfiguration
            {
                Disabled = false,
                DeleteFrequencyInSec = 60
            });

            documentStore.Maintenance.Send(configureExpirationOperation);
        }

        /// <summary>
        /// Sets the auditable entity values when the before store event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void SetAuditableEntityValues_OnBeforeStore(object sender, BeforeStoreEventArgs eventArgs)
        {
            if (eventArgs.Entity is not IAuditableEntity auditableEntity)
            {
                return;
            }

            if (auditableEntity.CreatedOnUtc == default)
            {
                ApplySetPropertyMethod(auditableEntity, SetCreatedOnMethodsDictionary, nameof(IAuditableEntity.CreatedOnUtc));
            }
            else if (auditableEntity.ModifiedOnUtc is null)
            {
                ApplySetPropertyMethod(auditableEntity, SetModifiedOnMethodsDictionary, nameof(IAuditableEntity.ModifiedOnUtc));
            }
        }

        /// <summary>
        /// Applies the set property method for the specified auditable entity.
        /// </summary>
        /// <param name="auditableEntity">The auditable entity.</param>
        /// <param name="propertySetterMethodsDictionary">The dictionary containing the set property methods.</param>
        /// <param name="propertyName">The property name.</param>
        private void ApplySetPropertyMethod(
            IAuditableEntity auditableEntity,
            IDictionary<Type, MethodInfo> propertySetterMethodsDictionary,
            string propertyName)
        {
            Type underlyingType = auditableEntity.GetType();

            if (!propertySetterMethodsDictionary.ContainsKey(underlyingType))
            {
                propertySetterMethodsDictionary.Add(underlyingType, underlyingType.GetProperty(propertyName)!.GetSetMethod(true)!);
            }

            propertySetterMethodsDictionary[underlyingType].Invoke(auditableEntity, new object[] { _dateTime.UtcNow });
        }
    }
}
