using MediatR;
using Order.Api.Dal.Interfaces;
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
        private readonly IOrderRepository _orderRepo;
        public CreateOrderCommandHandler(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var id = await _orderRepo.Create(new OrderEntity() { ConsumerFullName = request.ConsumerFullName, ConsumerAddress = request.ConsumerAddress });

            return new CreateOrderCommandResponse
            {
                IsSuccess = true,
                OrderId = id
            };
        }
    }
}
