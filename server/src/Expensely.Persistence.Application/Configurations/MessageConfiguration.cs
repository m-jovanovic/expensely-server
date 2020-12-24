using Expensely.Messaging.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Application.Configurations
{
    /// <summary>
    /// Contains the <see cref="Message"/> entity configuration.
    /// </summary>
    internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(message => message.Id);

            builder.Property(message => message.Name).HasMaxLength(200).IsRequired();

            builder.Property(message => message.Content).IsRequired();

            builder.Property(message => message.Processed).IsRequired().HasDefaultValue(false);

            builder.Property(message => message.Retries).IsRequired().HasDefaultValue(0);

            builder.Property(message => message.CreatedOnUtc).IsRequired();

            builder.Property(message => message.ModifiedOnUtc).IsRequired(false);
        }
    }
}
