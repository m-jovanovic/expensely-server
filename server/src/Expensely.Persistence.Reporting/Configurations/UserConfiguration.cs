using Expensely.Domain.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Reporting.Configurations
{
    /// <summary>
    /// Contains the <see cref="User"/> entity configuration.
    /// </summary>
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<User> builder) =>
            builder.ToTable(nameof(User), t => t.ExcludeFromMigrations());
    }
}
