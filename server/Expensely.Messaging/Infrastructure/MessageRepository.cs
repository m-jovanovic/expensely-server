using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Abstractions.Data;
using Expensely.Common.Clock;
using Expensely.Messaging.Abstractions;

namespace Expensely.Messaging.Infrastructure
{
    /// <summary>
    /// Represents the message repository.
    /// </summary>
    public sealed class MessageRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="dbConnectionProvider">The database connection provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public MessageRepository(IDbConnectionProvider dbConnectionProvider, IDateTime dateTime)
        {
            _dbConnectionProvider = dbConnectionProvider;
            _dateTime = dateTime;
        }

        /// <summary>
        /// Gets the specified number of unprocessed messages.
        /// </summary>
        /// <param name="take">The number of messages to take.</param>
        /// <returns>The specified number of unprocessed messages.</returns>
        public async Task<IEnumerable<Message>> GetUnprocessedAsync(int take)
        {
            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"SELECT TOP(@Take) * FROM [Message] WHERE Processed = 0 ORDER BY CreatedOnUtc";

            IEnumerable<Message> messages = await dbConnection.QueryAsync<Message>(sql, new
            {
                Take = take
            });

            return messages;
        }

        /// <summary>
        /// Checks if a consumer exists for the specified message and consumer name.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="consumerName">The consumer name.</param>
        /// <returns>True if a message consumer exists, otherwise false.</returns>
        public async Task<bool> CheckIfConsumerExistsAsync(Message message, string consumerName)
        {
            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"SELECT 1 FROM [MessageConsumer] WHERE MessageId = @MessageId AND ConsumerName = @ConsumerName";

            bool messageConsumerExists = await dbConnection.ExecuteScalarAsync<bool>(sql, new
            {
                MessageId = message.Id,
                ConsumerName = consumerName
            });

            return messageConsumerExists;
        }

        /// <summary>
        /// Inserts the specified message consumer to the database.
        /// </summary>
        /// <param name="messageConsumer">The message consumer.</param>
        /// <returns>The completed task.</returns>
        public async Task InsertConsumerAsync(MessageConsumer messageConsumer)
        {
            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"INSERT INTO [MessageConsumer](MessageId, ConsumerName) VALUES (@MessageId, @ConsumerName)";

            await dbConnection.ExecuteAsync(sql, messageConsumer);
        }

        /// <summary>
        /// Marks the specified message as processed.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The completed task.</returns>
        public async Task MarkAsProcessedAsync(Message message)
        {
            using IDbConnection dbConnection = _dbConnectionProvider.Create();

            const string sql = @"UPDATE [Message] SET Processed = 1, ModifiedOnUtc = @ModifiedOnUtc WHERE Id = @MessageId";

            await dbConnection.ExecuteAsync(sql, new
            {
                MessageId = message.Id,
                ModifiedOnUtc = _dateTime.UtcNow
            });
        }
    }
}
