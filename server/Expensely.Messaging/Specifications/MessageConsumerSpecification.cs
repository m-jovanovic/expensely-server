using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Messaging.Abstractions;

namespace Expensely.Messaging.Specifications
{
    /// <summary>
    /// Represents the specification for determining a message consumer.
    /// </summary>
    internal sealed class MessageConsumerSpecification : Specification<MessageConsumer>
    {
        private readonly Guid _messageId;
        private readonly string _consumerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageConsumerSpecification"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="consumerName">The consumer name.</param>
        public MessageConsumerSpecification(Message message, string consumerName)
        {
            _messageId = message.Id;
            _consumerName = consumerName;
        }

        /// <inheritdoc />
        public override Expression<Func<MessageConsumer, bool>> ToExpression() =>
            messageConsumer => messageConsumer.MessageId == _messageId && messageConsumer.ConsumerName == _consumerName;
    }
}
