using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Email;
using Expensely.Application.Contracts.Email;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages.Events;
using Microsoft.Extensions.Options;

namespace Expensely.Application.Events.Handlers.Messages
{
    /// <summary>
    /// Represents the <see cref="MessageRetryCountExceededEvent"/> handler.
    /// </summary>
    public sealed class SendNotificationEmail_MessageRetryCountExceededEventHandler : IEventHandler<MessageRetryCountExceededEvent>
    {
        private readonly NotificationSettings _settings;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendNotificationEmail_MessageRetryCountExceededEventHandler"/> class.
        /// </summary>
        /// <param name="notificationSettingsOptions">The notification settings options.</param>
        /// <param name="emailSender">The email sender.</param>
        public SendNotificationEmail_MessageRetryCountExceededEventHandler(
            IOptions<NotificationSettings> notificationSettingsOptions,
            IEmailSender emailSender)
        {
            _settings = notificationSettingsOptions.Value;
            _emailSender = emailSender;
        }

        /// <inheritdoc />
        public async Task Handle(MessageRetryCountExceededEvent @event, CancellationToken cancellationToken = default)
        {
            var mailRequest = new MailRequest
            {
                RecipientEmail = _settings.EmailRecipient,
                Subject = "Expensely - Message Retry Count Exceeded",
                Body = $"Message with identifier {@event.MessageId} has exceeded the allowed retry count."
            };

            await _emailSender.SendAsync(mailRequest, cancellationToken);
        }
    }
}
