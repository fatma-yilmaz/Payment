using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Api.Core.Validators;
using Payment.Api.Data.HttpClients;
using Payment.Api.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.Queries
{

    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>
    {
        private readonly PaymentDbContext _dbContext;
        private readonly OrderHttpClient _orderHttpClient;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public GetPaymentByIdQueryHandler(PaymentDbContext dbContext, OrderHttpClient orderHttpClient, IMediator mediator, IMapper mapper)
        {
            _dbContext = dbContext;
            _orderHttpClient = orderHttpClient;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<GetPaymentByIdQueryResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            //get payment details
            var payment = await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == request.Id);
            if (payment == null) return null;

            //get order details
            var order = new OrderDto { };
            if(CustomValidators.IsValidGuid(payment.OrderId))
            {
                var getOrderResponse = await _orderHttpClient.GetOrder(payment.OrderId, cancellationToken);
                order = _mapper.Map<OrderDto>(getOrderResponse);
            }

            var response = _mapper.Map<GetPaymentByIdQueryResponse>(payment);
            response.Order = order;

            return response;
        }
    }
}
