using CleanArc.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArc.Infrastructure.Persistence.Configuration.UserConfig;

internal class UserTokenConfig:IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {

        builder.HasOne(u => u.User).WithMany(u => u.Tokens).HasForeignKey(u => u.UserId);
        builder.ToTable("UserTokens","usr");
    }
}