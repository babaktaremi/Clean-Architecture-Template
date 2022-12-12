using CleanArc.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArc.Infrastructure.Persistence.Configuration.UserConfig;

internal class RefreshTokenConfig:IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.HasOne(c => c.User).WithMany(c => c.UserRefreshTokens).HasForeignKey(c => c.UserId);

        builder.ToTable("UserRefreshTokens", "usr");
    }
}