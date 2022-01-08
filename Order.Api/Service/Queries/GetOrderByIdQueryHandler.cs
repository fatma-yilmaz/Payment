using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.Dal.Interfaces;
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
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public GetOrderByIdQueryHandler(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetById(request.Id);

            return order == null ? null : _mapper.Map<GetOrderByIdQueryResponse>(order);
        }
    }
}
