using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Application.Configurations
{
    /// <summary>
    /// Contains the <see cref="Expense"/> entity configuration.
    /// </summary>
    internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Expense> builder) => builder.HasKey(expense => expense.Id);
    }
}
