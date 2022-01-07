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
    public class UpdatePaymentOrderStatusCommandHandler: IRequestHandler<UpdatePaymentOrderStatusCommand, UpdatePaymentOrderStatusCommandResponse>
    {
        private readonly PaymentDbContext _dbContext;
        public UpdatePaymentOrderStatusCommandHandler(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdatePaymentOrderStatusCommandResponse> Handle(UpdatePaymentOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == request.PaymentId);

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
            _dbContext.Payments.Update(payment);

            await _dbContext.SaveChangesAsync();

            return new UpdatePaymentOrderStatusCommandResponse
            {
                IsSuccess = true
            };
        }
    }
}
