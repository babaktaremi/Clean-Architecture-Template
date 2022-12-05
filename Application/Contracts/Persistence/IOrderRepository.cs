using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order;

namespace Application.Contracts.Persistence
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetAllUserOrdersAsync(int userId);
    }
}
