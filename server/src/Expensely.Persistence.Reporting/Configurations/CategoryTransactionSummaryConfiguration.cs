using Expensely.Domain.Reporting.Transactions;
using Expensely.Domain.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Reporting.Configurations
{
    /// <summary>
    /// Contains the <see cref="CategoryTransactionSummary"/> entity configuration.
    /// </summary>
    internal sealed class CategoryTransactionSummaryConfiguration : IEntityTypeConfiguration<CategoryTransactionSummary>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<CategoryTransactionSummary> builder)
        {
            builder.HasKey(categoryTransactionSummary => categoryTransactionSummary.Id).IsClustered(false);

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.UserId).IsRequired();

            builder.HasIndex(categoryTransactionSummary => categoryTransactionSummary.UserId).IsClustered();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.Year).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.Month).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.Category).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.Currency).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.Amount).HasPrecision(12, 4).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.TransactionType).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.CreatedOnUtc).IsRequired();

            builder.Property(categoryTransactionSummary => categoryTransactionSummary.ModifiedOnUtc).IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(categoryTransactionSummary => categoryTransactionSummary.UserId)
                .IsRequired();
        }
    }
}
