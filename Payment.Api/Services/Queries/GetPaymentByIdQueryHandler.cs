using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Api.Core.Validators;
using Payment.Api.Dal.Interfaces;
using Payment.Api.Data.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.Queries
{

    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly OrderHttpClient _orderHttpClient;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepo, OrderHttpClient orderHttpClient, IMediator mediator, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _orderHttpClient = orderHttpClient;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<GetPaymentByIdQueryResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            //get payment details
            var payment = await _paymentRepo.GetById(request.Id);
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
