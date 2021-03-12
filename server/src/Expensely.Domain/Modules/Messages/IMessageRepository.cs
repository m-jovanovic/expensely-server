using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Domain.Modules.Messages
{
    /// <summary>
    /// Represents the message repository.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Gets the message with the specified identifier, if one exists.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The maybe instance that may contain the message with the specified identifier.</returns>
        Task<Maybe<Message>> GetByIdAsync(Ulid messageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the specified number of unprocessed messages.
        /// </summary>
        /// <param name="numberOfMessages">The number of messages to get.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The specified number of unprocessed messages, if any exist.</returns>
        Task<IReadOnlyCollection<Message>> GetUnprocessedAsync(int numberOfMessages, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified message to the repository.
        /// </summary>
        /// <param name="message">The message to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(Message message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the specified message from the repository.
        /// </summary>
        /// <param name="message">The message to be removed.</param>
        void Remove(Message message);
    }
}
