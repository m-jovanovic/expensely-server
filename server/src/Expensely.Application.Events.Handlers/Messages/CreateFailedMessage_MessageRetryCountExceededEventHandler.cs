using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Expensely.Domain.Modules.Messages.Events;

namespace Expensely.Application.Events.Handlers.Messages
{
    /// <summary>
    /// Represents the <see cref="MessageRetryCountExceededEvent"/> handler.
    /// </summary>
    public sealed class CreateFailedMessage_MessageRetryCountExceededEventHandler : EventHandler<MessageRetryCountExceededEvent>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IFailedMessageRepository _failedMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFailedMessage_MessageRetryCountExceededEventHandler"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="failedMessageRepository">The failed message repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateFailedMessage_MessageRetryCountExceededEventHandler(
            IMessageRepository messageRepository,
            IFailedMessageRepository failedMessageRepository,
            IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _failedMessageRepository = failedMessageRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public override async Task Handle(MessageRetryCountExceededEvent @event, CancellationToken cancellationToken = default)
        {
            Maybe<Message> maybeMessage = await _messageRepository.GetByIdAsync(@event.MessageId, cancellationToken);

            if (maybeMessage.HasNoValue)
            {
                return;
            }

            var failedMessage = new FailedMessage(maybeMessage.Value);

            await _failedMessageRepository.AddAsync(failedMessage, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
