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
        public Task<Guid> Create(OrderEntity order)
        {
            var id = Guid.NewGuid();
            order.Id = id;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(id);
        }

        public Task<OrderEntity> GetById(Guid id)
        {
            return _dbContext.Orders.SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
