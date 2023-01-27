using CleanArc.Domain.Entities.Order;

namespace CleanArc.Application.Contracts.Persistence;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<List<Order>> GetAllUserOrdersAsync(int userId);
    Task<List<Order>> GetAllOrdersWithRelatedUserAsync();
}