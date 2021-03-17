using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Expensely.BackgroundTasks.MessageProcessing.Abstractions
{
    /// <summary>
    /// Represents the message processing job interface.
    /// </summary>
    public interface IMessageProcessingJob : IJob
    {
        /// <summary>
        /// Gets the message processing job name.
        /// </summary>
        static string Name => "MessageProcessingJob";

        /// <summary>
        /// Processes the unprocessed messages.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task ProcessMessagesAsync(CancellationToken cancellationToken);
    }
}
