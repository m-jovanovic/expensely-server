using Expensely.Domain.Reporting.Transactions;
using Expensely.Domain.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Reporting.Configurations
{
    /// <summary>
    /// Contains the <see cref="TransactionSummary"/> entity configuration.
    /// </summary>
    internal sealed class TransactionSummaryConfiguration : IEntityTypeConfiguration<TransactionSummary>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<TransactionSummary> builder)
        {
            builder.HasKey(transactionSummary => transactionSummary.Id).IsClustered(false);

            builder.Property(transactionSummary => transactionSummary.UserId).IsRequired();

            builder.HasIndex(transactionSummary => transactionSummary.UserId).IsClustered();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(transactionSummary => transactionSummary.UserId)
                .IsRequired();

            builder.Property(transactionSummary => transactionSummary.Year).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Month).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Currency).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Amount).HasPrecision(12, 4).IsRequired();

            builder.Property(transactionSummary => transactionSummary.TransactionType).IsRequired();

            builder.Property(transactionSummary => transactionSummary.CreatedOnUtc).IsRequired();

            builder.Property(transactionSummary => transactionSummary.ModifiedOnUtc).IsRequired(false);
        }
    }
}
