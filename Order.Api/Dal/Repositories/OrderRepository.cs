using Microsoft.EntityFrameworkCore;
using Order.Api.Dal.Interfaces;
using Order.Api.DBContexts;
using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Dal.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly OrderDbContext _dbContext;
        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(OrderEntity order)
        {
            var id = Guid.NewGuid();
            order.Id = id;
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return id;
        }

        public async Task<OrderEntity> GetById(Guid id)
        {
            return await _dbContext.Orders.SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
