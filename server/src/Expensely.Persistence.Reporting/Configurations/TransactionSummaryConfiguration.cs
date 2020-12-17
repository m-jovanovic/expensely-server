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
            builder.HasKey(transactionSummary => transactionSummary.Id);

            builder.Property(transactionSummary => transactionSummary.UserId).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Year).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Month).IsRequired();

            builder.Property(transactionSummary => transactionSummary.TransactionType).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Currency).IsRequired();

            builder.Property(transactionSummary => transactionSummary.Amount).HasPrecision(12, 4).IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
