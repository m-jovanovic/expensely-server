using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Notification;
using Expensely.Application.Contracts.Notification;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages.Events;

namespace Expensely.Application.Events.Handlers.Messages
{
    /// <summary>
    /// Represents the <see cref="MessageRetryCountExceededEvent"/> handler.
    /// </summary>
    public sealed class SendNotificationEmail_MessageRetryCountExceededEventHandler : EventHandler<MessageRetryCountExceededEvent>
    {
        private readonly IAlertSender _alertSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendNotificationEmail_MessageRetryCountExceededEventHandler"/> class.
        /// </summary>
        /// <param name="alertSender">The alert sender.</param>
        public SendNotificationEmail_MessageRetryCountExceededEventHandler(IAlertSender alertSender) => _alertSender = alertSender;

        /// <inheritdoc />
        public override async Task Handle(MessageRetryCountExceededEvent @event, CancellationToken cancellationToken = default)
        {
            var mailRequest = new AlertRequest
            {
                Subject = "Expensely - Message Retry Count Exceeded",
                Body = $"Message with identifier {@event.MessageId} has exceeded the allowed retry count."
            };

            await _alertSender.SendAsync(mailRequest, cancellationToken);
        }
    }
}
