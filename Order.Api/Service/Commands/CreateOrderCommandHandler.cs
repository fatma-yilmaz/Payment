using MediatR;
using Order.Api.Data.Commands;
using Order.Api.DBContexts;
using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Api.Data.Commands
{
    public class CreateOrderCommandHandler :IRequestHandler<CreateOrderCommand,CreateOrderCommandResponse>
    {
        private readonly OrderDbContext _dbContext;
        public CreateOrderCommandHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
             _dbContext.Orders.Add(new OrderEntity()
            {
                Id = id,
                ConsumerFullName = request.ConsumerFullName,
                ConsumerAddress = request.ConsumerAddress
            });
            await _dbContext.SaveChangesAsync();
            return new CreateOrderCommandResponse
            {
                IsSuccess = true,
                OrderId = id
            };
        }
    }
}
