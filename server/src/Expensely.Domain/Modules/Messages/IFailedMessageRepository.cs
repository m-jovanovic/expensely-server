using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Domain.Modules.Messages
{
    /// <summary>
    /// Represents the failed message repository.
    /// </summary>
    public interface IFailedMessageRepository
    {
        /// <summary>
        /// Adds the specified failed message to the repository.
        /// </summary>
        /// <param name="failedMessage">The failed message to be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task AddAsync(FailedMessage failedMessage, CancellationToken cancellationToken = default);
    }
}
