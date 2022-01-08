using Microsoft.EntityFrameworkCore;
using Payment.Api.Dal.Interfaces;
using Payment.Api.DBContexts;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<Guid> Create(PaymentEntity payment)
        {
            var id = Guid.NewGuid();
            payment.Id = id;
            _dbContext.Payments.Add(payment);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(id);
        }

        public Task<PaymentEntity> GetById(Guid id)
        {
            return _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == id);
        }

        public Task<bool> Update(PaymentEntity payment)
        {
            _dbContext.Payments.Update(payment);
            _dbContext.SaveChangesAsync();
            return Task.FromResult(true);

        }
    }
}
