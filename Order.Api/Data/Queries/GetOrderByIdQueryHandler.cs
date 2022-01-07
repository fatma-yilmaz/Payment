using AutoMapper;
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
        private readonly IMapper _mapper;
        public GetOrderByIdQueryHandler(OrderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.SingleOrDefaultAsync(p => p.Id == request.Id);

            return order == null ? null : _mapper.Map<GetOrderByIdQueryResponse>(order);
        }
    }
}
