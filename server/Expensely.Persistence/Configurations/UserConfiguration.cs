using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="User"/> entity configuration.
    /// </summary>
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(email => email.Value).HasColumnName(nameof(User.Email)).HasMaxLength(Email.MaxLength).IsRequired();

                emailBuilder.HasIndex(email => email.Value).IsUnique();
            });

            builder.Property(transaction => transaction.CreatedOnUtc).IsRequired();

            builder.Property(transaction => transaction.ModifiedOnUtc).IsRequired(false);
        }
    }
}
