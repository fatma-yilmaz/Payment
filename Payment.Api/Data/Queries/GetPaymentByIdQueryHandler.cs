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
        public GetPaymentByIdQueryHandler(PaymentDbContext dbContext, OrderHttpClient orderHttpClient, IMediator mediator)
        {
            _dbContext = dbContext;
            _orderHttpClient = orderHttpClient;
            _mediator = mediator;
        }
        public async Task<GetPaymentByIdQueryResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetPaymentByIdQueryResponse { };

            //get payment details
            var payment = await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == request.Id);
            if (payment == null) return null;

            //get order details
            var order = new Order { };
            if(CustomValidators.BeAValidGuid(payment.OrderId))
            {
                var getOrderResponse = await _orderHttpClient.GetOrder(payment.OrderId, cancellationToken);
                order.Id = getOrderResponse.id;
                order.ConsumerFullName = getOrderResponse.consumerFullName;
                order.ConsumerAddress = getOrderResponse.consumerAddress;
            }
            
            return new GetPaymentByIdQueryResponse
            {
                Id = payment.Id,
                CreationDate = payment.CreationDate,
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                Status =payment.Status ,
                Order = order
            };
        }
    }
}
