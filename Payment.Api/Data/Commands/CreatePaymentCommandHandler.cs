using MediatR;
using Payment.Api.Data.HttpClients;
using Payment.Api.DBContexts;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class CreatePaymentCommandHandler :IRequestHandler<CreatePaymentCommand,CreatePaymentCommandResponse>
    {
        private readonly PaymentDbContext _dbContext;
        private readonly OrderHttpClient _orderHttpClient;
        private readonly IMediator _mediator;
        public CreatePaymentCommandHandler(PaymentDbContext dbContext, OrderHttpClient orderHttpClient, IMediator mediator)
        {
            _dbContext = dbContext;
            _orderHttpClient = orderHttpClient;
            _mediator = mediator;
        }
        public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            //create payment
            var id = Guid.NewGuid();
             _dbContext.Payments.Add(new PaymentEntity()
            {
                Id = id,
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode,
                Status = "Created",
                CreationDate = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();

            //create order
            var orderResponse = await _orderHttpClient.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken);
            
            if(!orderResponse.isSuccess)
            {
                //update payment status to Failed and return
                var updatePaymentStatusResponse = await _mediator.Send(new UpdatePaymentStatusCommand() { PaymentId = id, Status = "Failed" });

                return new CreatePaymentCommandResponse{IsSuccess = false};
            }

            //update orderId of payment and set payment status to Completed
            var updatePaymentResponse = await _mediator.Send(new UpdatePaymentOrderStatusCommand() { PaymentId = id, OrderId = orderResponse.orderId });
            
            return new CreatePaymentCommandResponse
            {
                IsSuccess = updatePaymentResponse.IsSuccess,
                PaymenId = id
            };
        }
    }
}
