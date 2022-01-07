using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly PaymentDbContext _dbContext;
        public UpdatePaymentStatusCommandHandler(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdatePaymentStatusCommandResponse> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == request.PaymentId);

            if (payment == null)
            {
                return new UpdatePaymentStatusCommandResponse
                {
                    IsSuccess = false,
                    Message = "payment not found"
                };

            }

            payment.Status = request.Status;
            _dbContext.Payments.Update(payment);
            await _dbContext.SaveChangesAsync();

            return new UpdatePaymentStatusCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
