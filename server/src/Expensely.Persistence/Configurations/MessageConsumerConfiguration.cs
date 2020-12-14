using Expensely.Messaging.Abstractions;
using Expensely.Messaging.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="MessageConsumer"/> entity configuration.
    /// </summary>
    internal sealed class MessageConsumerConfiguration : IEntityTypeConfiguration<MessageConsumer>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<MessageConsumer> builder)
        {
            builder.HasKey(message => new { message.MessageId, message.ConsumerName });

            builder.Property(messageConsumer => messageConsumer.ConsumerName).HasMaxLength(200);

            builder.HasOne<Message>()
                .WithMany()
                .HasForeignKey(messageConsumer => messageConsumer.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
