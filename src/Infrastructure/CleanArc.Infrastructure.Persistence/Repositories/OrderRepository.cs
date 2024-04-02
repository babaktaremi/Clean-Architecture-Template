using CleanArc.Application.Contracts.Persistence;
using CleanArc.Domain.Entities.Order;
using CleanArc.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArc.Infrastructure.Persistence.Repositories;

internal class OrderRepository(ApplicationDbContext dbContext) : BaseAsyncRepository<Order>(dbContext), IOrderRepository
{
    public async Task AddOrderAsync(Order order)
    {
        await base.AddAsync(order);
    }

    public async Task<List<Order>> GetAllUserOrdersAsync(int userId)
    {
        return await base.TableNoTracking.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<List<Order>> GetAllOrdersWithRelatedUserAsync()
    {
        var orders = await base.TableNoTracking.Include(c => c.User).ToListAsync();

        return orders;
    }

    public async Task<Order> GetUserOrderByIdAndUserIdAsync(int userId, int orderId,bool trackEntity)
    {
        var order = await base.TableNoTracking.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == orderId);

        if (order is not null && trackEntity)
            base.DbContext.Attach(order);

        return order;
    }

    public async Task DeleteUserOrdersAsync(int userId)
    {
        await UpdateAsync(c => c.UserId == userId, p => p.SetProperty(order => order.IsDeleted, true));
    }
}