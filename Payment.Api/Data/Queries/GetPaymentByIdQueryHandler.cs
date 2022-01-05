using MediatR;
using Microsoft.EntityFrameworkCore;
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
        public GetPaymentByIdQueryHandler(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetPaymentByIdQueryResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == request.Id);
            return new GetPaymentByIdQueryResponse
            {
                Id = payment.Id,
                CreationDate = payment.CreationDate,
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                Status =payment.Status              
            };
        }
    }
}
