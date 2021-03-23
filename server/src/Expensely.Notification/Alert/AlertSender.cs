using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Notification;
using Expensely.Application.Contracts.Notification;
using Expensely.Common.Primitives.ServiceLifetimes;
using Microsoft.Extensions.Options;

namespace Expensely.Notification.Alert
{
    /// <summary>
    /// Represents the alert sender.
    /// </summary>
    internal sealed class AlertSender : IAlertSender, ITransient
    {
        private readonly IEmailSender _emailSender;
        private readonly AlertSettings _alertSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertSender"/> class.
        /// </summary>
        /// <param name="emailSender">The email sender.</param>
        /// <param name="alertSettingsOptions">The alert settings options.</param>
        public AlertSender(IEmailSender emailSender, IOptions<AlertSettings> alertSettingsOptions)
        {
            _emailSender = emailSender;
            _alertSettings = alertSettingsOptions.Value;
        }

        /// <inheritdoc />
        public async Task SendAsync(AlertRequest alertRequest, CancellationToken cancellationToken = default)
        {
            var mailRequest = new MailRequest
            {
                RecipientEmail = _alertSettings.EmailRecipient,
                Subject = alertRequest.Subject,
                Body = alertRequest.Body
            };

            await _emailSender.SendAsync(mailRequest, cancellationToken);
        }
    }
}
