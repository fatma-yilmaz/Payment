using MediatR;
using Payment.Api.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class UpdatePaymentOrderStatusCommandHandler: IRequestHandler<UpdatePaymentOrderStatusCommand, UpdatePaymentOrderStatusCommandResponse>
    {
        private readonly IPaymentRepository _paymentRepo;
        public UpdatePaymentOrderStatusCommandHandler(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<UpdatePaymentOrderStatusCommandResponse> Handle(UpdatePaymentOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepo.GetById(request.PaymentId);

            if(payment ==null)
            {
                return new UpdatePaymentOrderStatusCommandResponse
                {
                    IsSuccess = false,
                    Message = "payment not found"
                };

            }

            payment.OrderId = request.OrderId;
            payment.Status = "Completed";
            await _paymentRepo.Update(payment);

            return new UpdatePaymentOrderStatusCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
