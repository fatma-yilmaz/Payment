using MediatR;
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
        public CreatePaymentCommandHandler(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
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
            return new CreatePaymentCommandResponse
            {
                IsSuccess = true,
                PaymenId = id
            };
        }
    }
}
