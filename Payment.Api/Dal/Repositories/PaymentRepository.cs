using Microsoft.EntityFrameworkCore;
using Payment.Api.Dal.IRepositories;
using Payment.Api.DBContexts;
using Payment.Api.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _dbContext;
        public PaymentRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Create(PaymentEntity payment, CancellationToken cancellationToken)
        {
            payment.Id = Guid.NewGuid();
            await _dbContext.Payments.AddAsync(payment,cancellationToken);
            return payment.Id;
        }

        public async Task<PaymentEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            return  await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task Update(PaymentEntity payment, CancellationToken cancellationToken)
        {
            _dbContext.Payments.Update(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
