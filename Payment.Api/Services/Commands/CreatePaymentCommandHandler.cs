using MediatR;
using Payment.Api.Dal.Interfaces;
using Payment.Api.Data.HttpClients;
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
        private readonly IPaymentRepository _paymentRepo;
        private readonly OrderHttpClient _orderHttpClient;
        private readonly IMediator _mediator;
        public CreatePaymentCommandHandler(IPaymentRepository paymentRepo, OrderHttpClient orderHttpClient, IMediator mediator)
        {
            _paymentRepo = paymentRepo;
            _orderHttpClient = orderHttpClient;
            _mediator = mediator;
        }
        public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            //create payment
            var id=await _paymentRepo.Create(new PaymentEntity()
                                    {
                                        Amount = request.Amount,
                                        CurrencyCode = request.CurrencyCode,
                                        Status = "Created",
                                        CreationDate = DateTime.Now
                                    });

            //create order
            var orderResponse = await _orderHttpClient.CreateOrder(request.Order.ConsumerFullName, request.Order.ConsumerAddress, cancellationToken);

            if (!orderResponse.isSuccess)
            {
                //update payment status to Failed and return
                var updatePaymentStatusResponse = await _mediator.Send(new UpdatePaymentStatusCommand(paymentId: id, status: "Failed"),cancellationToken);
                return new CreatePaymentCommandResponse { IsSuccess = false };
            }

            //update orderId of payment and set payment status to Completed
            var updatePaymentResponse = await _mediator.Send(new UpdatePaymentOrderStatusCommand(paymentId: id, orderId: orderResponse.orderId),cancellationToken);

            return new CreatePaymentCommandResponse
            {
                IsSuccess = updatePaymentResponse.IsSuccess,
                PaymenId = id
            };
        }
    }
}
