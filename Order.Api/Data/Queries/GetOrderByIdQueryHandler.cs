using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Api.Data.Queries
{

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdQueryResponse>
    {
        private readonly OrderDbContext _dbContext;
        public GetOrderByIdQueryHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.SingleOrDefaultAsync(p => p.Id == request.Id);

            return order ==null ? null : new GetOrderByIdQueryResponse
            {
                Id = order.Id,
                ConsumerFullName = order.ConsumerFullName,
                ConsumerAddress = order.ConsumerAddress        
            };
        }
    }
}
