using CleanArc.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArc.Infrastructure.Persistence.Configuration.UserConfig;

internal class RoleClaimConfig:IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims","usr");
           
        builder.HasOne(u => u.Role).WithMany(u => u.Claims).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Cascade);


    }
}