using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Notification;

namespace Expensely.Application.Abstractions.Notification
{
    /// <summary>
    /// Represents the alert sender interface.
    /// </summary>
    public interface IAlertSender
    {
        /// <summary>
        /// Sends an alert with the specified parameters.
        /// </summary>
        /// <param name="alertRequest">The alert request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task SendAsync(AlertRequest alertRequest, CancellationToken cancellationToken = default);
    }
}
