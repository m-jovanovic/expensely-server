using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="Transaction"/> entity configuration.
    /// </summary>
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasDiscriminator<int>(nameof(TransactionType))
                .HasValue<Expense>((int)TransactionType.Expense)
                .HasValue<Income>((int)TransactionType.Income);

            builder.HasIndex(nameof(TransactionType));

            builder.OwnsOne(transaction => transaction.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(money => money.Amount).HasColumnName(nameof(Money.Amount)).HasPrecision(12, 4).IsRequired();

                moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(currency => currency.Value).HasColumnName(nameof(Money.Currency)).IsRequired();

                    currencyBuilder.Ignore(currency => currency.Code);

                    currencyBuilder.Ignore(currency => currency.Name);
                });
            });

            builder.Property(transaction => transaction.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(transaction => transaction.CreatedOnUtc).IsRequired();

            builder.Property(transaction => transaction.ModifiedOnUtc).IsRequired(false);
        }
    }
}
