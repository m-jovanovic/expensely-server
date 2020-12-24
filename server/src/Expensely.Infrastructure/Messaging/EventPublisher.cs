using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Events;
using Expensely.Messaging.Abstractions.Entities;
using Newtonsoft.Json;

namespace Expensely.Infrastructure.Messaging
{
    /// <summary>
    /// Represents the event publisher.
    /// </summary>
    public sealed class EventPublisher : IEventPublisher
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public EventPublisher(IDbConnectionProvider dbConnectionProvider, IDateTime dateTime)
        {
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task PublishAsync(IEvent @event, IDbTransaction transaction = null, CancellationToken cancellationToken = default) =>
            await PublishAsync(new[] { @event }, transaction, cancellationToken);

        /// <inheritdoc />
        public async Task PublishAsync(
            IEnumerable<IEvent> events, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            Message[] messages = events.Select(ConvertEventToMessage).ToArray();

            if (!messages.Any())
            {
                return;
            }

            if (transaction?.Connection is not null)
            {
                await InsertMessages(transaction.Connection, messages, transaction);

                return;
            }

            using IDbConnection dbConnection = transaction?.Connection ?? _dbConnectionProvider.Create();

            await InsertMessages(dbConnection, messages);
        }

        private static Task InsertMessages(IDbConnection dbConnection, Message[] messages, IDbTransaction transaction = null) =>
            dbConnection.ExecuteAsync(
                "INSERT [Message] (Id, Name, Content, CreatedOnUtc) VALUES(@Id, @Name, @Content, @CreatedOnUtc)",
                messages,
                transaction);

        private Message ConvertEventToMessage(IEvent @event) =>
            new(
                @event.GetType().Name,
                JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                _dateTime.UtcNow);
    }
}
