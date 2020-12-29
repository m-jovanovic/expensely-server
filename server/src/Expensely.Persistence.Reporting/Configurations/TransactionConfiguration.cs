using Expensely.Domain.Reporting.Transactions;
using Expensely.Domain.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Reporting.Configurations
{
    /// <summary>
    /// Contains the <see cref="Transaction"/> entity configuration.
    /// </summary>
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable(nameof(Transaction), t => t.ExcludeFromMigrations());

            builder.HasKey(transaction => transaction.Id);

            // TODO: Implement support for categories in database.
            builder.Property(transaction => transaction.TransactionType).IsRequired();

            builder.Property(transaction => transaction.Currency).IsRequired();

            builder.Property(transaction => transaction.Amount).HasPrecision(12, 4).IsRequired();

            builder.Property(transaction => transaction.OccurredOn).HasColumnType("date").IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(transaction => transaction.UserId)
                .IsRequired();
        }
    }
}
