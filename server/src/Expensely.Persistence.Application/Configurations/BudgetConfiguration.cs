using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Application.Configurations
{
    /// <summary>
    /// Contains the <see cref="Budget"/> entity configuration.
    /// </summary>
    internal sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(budget => budget.Id);

            builder.OwnsOne(budget => budget.Name, nameBuilder =>
                nameBuilder
                    .Property(name => name.Value)
                    .HasColumnName(nameof(Budget.Name))
                    .HasMaxLength(Name.MaxLength)
                    .IsRequired());

            builder.OwnsOne(budget => budget.Money, moneyBuilder =>
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

            builder.Navigation(budget => budget.Money).IsRequired();

            builder.Navigation(budget => budget.Name).IsRequired();

            builder.Property(budget => budget.StartDate).HasColumnType("date").IsRequired();

            builder.Property(budget => budget.EndDate).HasColumnType("date").IsRequired();

            builder.Property(budget => budget.Expired).IsRequired();

            builder.Property(budget => budget.CreatedOnUtc).IsRequired();

            builder.Property(budget => budget.ModifiedOnUtc).IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(transaction => transaction.UserId)
                .IsRequired();
        }
    }
}
