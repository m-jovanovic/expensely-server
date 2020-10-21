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
        private const string PasswordHashField = "_passwordHash";
        private const string PasswordHashColumn = "PasswordHash";

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.OwnsOne(user => user.FirstName, firstNameBuilder =>
                firstNameBuilder
                    .Property(firstName => firstName.Value)
                    .HasColumnName(nameof(User.FirstName))
                    .HasMaxLength(FirstName.MaxLength)
                    .IsRequired());

            builder.OwnsOne(user => user.LastName, lastNameBuilder =>
                lastNameBuilder
                    .Property(lastName => lastName.Value)
                    .HasColumnName(nameof(User.LastName))
                    .HasMaxLength(LastName.MaxLength)
                    .IsRequired());

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.Property(email => email.Value).HasColumnName(nameof(User.Email)).HasMaxLength(Email.MaxLength).IsRequired();

                emailBuilder.HasIndex(email => email.Value).IsUnique();
            });

            builder.Navigation(user => user.FirstName).IsRequired();

            builder.Navigation(user => user.LastName).IsRequired();

            builder.Navigation(user => user.Email).IsRequired();

            builder.Property<string>(PasswordHashField)
                .HasField(PasswordHashField)
                .HasColumnName(PasswordHashColumn)
                .IsRequired();

            builder.Property(user => user.FullName)
                .HasComputedColumnSql($"[{nameof(User.FirstName)}] + ' ' + [{nameof(User.LastName)}]", true)
                .HasMaxLength(FirstName.MaxLength + LastName.MaxLength + 1)
                .IsRequired();

            builder.Property(transaction => transaction.CreatedOnUtc).IsRequired();

            builder.Property(transaction => transaction.ModifiedOnUtc).IsRequired(false);
        }
    }
}
