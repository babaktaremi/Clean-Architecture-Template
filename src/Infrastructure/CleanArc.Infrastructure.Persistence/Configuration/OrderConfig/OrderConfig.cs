using CleanArc.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArc.Infrastructure.Persistence.Configuration.OrderConfig;

internal class OrderConfig:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(c => c.User).WithMany(c => c.Orders).HasForeignKey(c => c.UserId);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}