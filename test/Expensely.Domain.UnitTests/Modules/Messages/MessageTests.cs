using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Messages;
using Expensely.Domain.Modules.Messages.Events;
using Expensely.Domain.Modules.Users.Events;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Messages
{
    public class MessageTests
    {
        private const string ConsumerName = "consumer";

        [Fact]
        public void Constructor_ShouldCreateMessage_WithProperValues()
        {
            // Arrange
            IEvent @event = new UserCreatedEvent();

            // Act
            var message = new Message(@event);

            // Assert
            message.Event.Should().NotBeNull();
            message.Processed.Should().BeFalse();
            message.RetryCount.Should().Be(0);
            message.CreatedOnUtc.Should().Be(default);
            message.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public void MarkAsProcessed_ShouldMarkMessageAsProcessed()
        {
            // Arrange
            var message = new Message(default);

            // Act
            message.MarkAsProcessed();

            // Assert
            message.Processed.Should().BeTrue();
        }

        [Fact]
        public void Retry_ShouldIncreaseRetryCountByOne()
        {
            // Arrange
            var message = new Message(default);

            int startingRetryCount = message.RetryCount;

            // Act
            message.Retry(2);

            // Assert
            message.RetryCount.Should().Be(startingRetryCount + 1);
        }

        [Fact]
        public void Retry_ShouldRaiseMessageRetryCountExceededEvent_WhenRetryCountThresholdIsExceeded()
        {
            // Arrange
            var message = new Message(default);

            // Act
            message.Retry(1);

            // Assert
            message.GetEvents().Should().AllBeOfType<MessageRetryCountExceededEvent>();
        }

        [Fact]
        public void Retry_ShouldMarkMessageAsProcessed_WhenRetryCountThresholdIsExceeded()
        {
            // Arrange
            var message = new Message(default);

            // Act
            message.Retry(1);

            // Assert
            message.Processed.Should().BeTrue();
        }

        [Fact]
        public void AddConsumer_ShouldAddConsumerToMessage()
        {
            // Arrange
            var message = new Message(default);

            int startingMessageConsumerCount = message.MessageConsumers.Count;

            // Act
            message.AddConsumer(ConsumerName, default);

            // Assert
            message.MessageConsumers.Should().HaveCount(startingMessageConsumerCount + 1);
        }

        [Fact]
        public void AddConsumer_ShouldNotAddMessageConsumer_WhenMessageConsumerWithSameNameWasPreviouslyAdded()
        {
            // Arrange
            var message = new Message(default);

            message.AddConsumer(ConsumerName, default);

            int startingMessageConsumerCount = message.MessageConsumers.Count;

            // Act
            message.AddConsumer(ConsumerName, default);

            // Assert
            message.MessageConsumers.Should().HaveCount(startingMessageConsumerCount);
        }

        [Fact]
        public void HasBeenProcessedBy_ShouldReturnFalse_WhenMessageConsumerHasNotBeenProcessed()
        {
            // Arrange
            var message = new Message(default);

            // Act
            bool result = message.HasBeenProcessedBy(ConsumerName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasBeenProcessedBy_ShouldReturnTrue_WhenMessageConsumerHasBeenProcessed()
        {
            // Arrange
            var message = new Message(default);

            message.AddConsumer(ConsumerName, default);

            // Act
            bool result = message.HasBeenProcessedBy(ConsumerName);

            // Assert
            result.Should().BeTrue();
        }
    }
}
