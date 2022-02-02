using Microsoft.EntityFrameworkCore;
using Payment.Api.Dal.IRepositories;
using Payment.Api.DBContexts;
using Payment.Api.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly PaymentDbContext _dbContext;
        public OrderRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(OrderEntity order, CancellationToken cancellationToken)
        {
            order.Id = Guid.NewGuid();
            await _dbContext.Orders.AddAsync(order,cancellationToken);
            return order.Id;
        }

        public async Task<OrderEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders.SingleOrDefaultAsync(p => p.Id == id,cancellationToken);
        }
    }
}
