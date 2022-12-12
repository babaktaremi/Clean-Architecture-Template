using CleanArc.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArc.Infrastructure.Persistence.Configuration.UserConfig;

internal class UserLoginConfig:IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.HasOne(u => u.User).WithMany(u => u.Logins).HasForeignKey(u => u.UserId);
        builder.ToTable("UserLogins","usr");
    }
}