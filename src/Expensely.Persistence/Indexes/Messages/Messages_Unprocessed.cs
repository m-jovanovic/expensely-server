using System.Linq;
using Expensely.Domain.Modules.Messages;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Messages
{
    /// <summary>
    /// Represents the index on the messages collection by processed field where processed is false.
    /// </summary>
    public sealed class Messages_Unprocessed : AbstractIndexCreationTask<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Messages_Unprocessed"/> class.
        /// </summary>
        public Messages_Unprocessed() =>
            Map = messages =>
                from message in messages
                where message.Processed == false
                select new
                {
                    message.Processed,
                    message.CreatedOnUtc
                };
    }
}
