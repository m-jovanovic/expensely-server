using Expensely.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Application.Configurations
{
    /// <summary>
    /// Represents the <see cref="RefreshToken"/> entity configuration.
    /// </summary>
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken));

            builder.HasKey(refreshToken => refreshToken.UserId);

            builder.Property(refreshToken => refreshToken.Token).HasMaxLength(100).IsRequired();

            builder.Property(refreshToken => refreshToken.ExpiresOnUtc).IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<RefreshToken>(refreshToken => refreshToken.UserId);

            builder.HasIndex(refreshToken => refreshToken.Token);
        }
    }
}
