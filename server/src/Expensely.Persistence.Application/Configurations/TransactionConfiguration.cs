using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Application.Configurations
{
    /// <summary>
    /// Contains the <see cref="Transaction"/> entity configuration.
    /// </summary>
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(transaction => transaction.Id);

            builder.HasDiscriminator<int>(nameof(TransactionType))
                .HasValue<Expense>((int)TransactionType.Expense)
                .HasValue<Income>((int)TransactionType.Income);

            builder.OwnsOne(transaction => transaction.Name, nameBuilder =>
                nameBuilder
                    .Property(name => name.Value)
                    .HasColumnName(nameof(Transaction.Name))
                    .HasMaxLength(Name.MaxLength)
                    .IsRequired());

            // TODO: Implement support for categories in database.
            builder.OwnsOne(transaction => transaction.Money, moneyBuilder =>
            {
                moneyBuilder.Property(money => money.Amount).HasColumnName(nameof(Money.Amount)).HasPrecision(12, 4).IsRequired();

                moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.Property(currency => currency.Value).HasColumnName(nameof(Money.Currency)).IsRequired();

                    currencyBuilder.Ignore(currency => currency.Code);

                    currencyBuilder.Ignore(currency => currency.Name);
                });

                moneyBuilder.Navigation(money => money.Currency).IsRequired();
            });

            builder.OwnsOne(transaction => transaction.Description, descriptionBuilder =>
                descriptionBuilder
                    .Property(name => name.Value)
                    .HasColumnName(nameof(Transaction.Description))
                    .HasMaxLength(Description.MaxLength)
                    .IsRequired());

            builder.Navigation(transaction => transaction.Money).IsRequired();

            builder.Navigation(transaction => transaction.Name).IsRequired();

            builder.Navigation(transaction => transaction.Description).IsRequired();

            builder.Property(transaction => transaction.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(transaction => transaction.CreatedOnUtc).IsRequired();

            builder.Property(transaction => transaction.ModifiedOnUtc).IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(transaction => transaction.UserId)
                .IsRequired();
        }
    }
}
