using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Notification;

namespace Expensely.Application.Abstractions.Notification
{
    /// <summary>
    /// Represents the email sender interface.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email message with the specified parameters.
        /// </summary>
        /// <param name="mailRequest">The mail request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task SendAsync(MailRequest mailRequest, CancellationToken cancellationToken = default);
    }
}
