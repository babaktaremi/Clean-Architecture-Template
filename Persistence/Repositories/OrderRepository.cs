using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    internal class OrderRepository:BaseAsyncRepository<Order>,IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddOrderAsync(Order order)
        {
            await base.AddAsync(order);
        }

        public async Task<List<Order>> GetAllUserOrdersAsync(int userId)
        {
            return await base.TableNoTracking.Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
