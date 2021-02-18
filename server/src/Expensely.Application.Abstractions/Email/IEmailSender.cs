using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions.Email
{
    /// <summary>
    /// Represents the email sender interface.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email message with the specified parameters.
        /// </summary>
        /// <param name="recipient">The email recipient.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isBodyHtml">The value indicating whether the body is HTML or not.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task SendAsync(string recipient, string subject, string body, bool isBodyHtml = false, CancellationToken cancellationToken = default);
    }
}
