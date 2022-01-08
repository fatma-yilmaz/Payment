using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Api.Dal.Interfaces;
using Payment.Api.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, UpdatePaymentStatusCommandResponse>
    {
        private readonly IPaymentRepository _paymentRepo;
        public UpdatePaymentStatusCommandHandler(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<UpdatePaymentStatusCommandResponse> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepo.GetById(request.PaymentId);

            if (payment == null)
            {
                return new UpdatePaymentStatusCommandResponse
                {
                    IsSuccess = false,
                    Message = "payment not found"
                };

            }

            payment.Status = request.Status;
            await _paymentRepo.Update(payment);

            return new UpdatePaymentStatusCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
