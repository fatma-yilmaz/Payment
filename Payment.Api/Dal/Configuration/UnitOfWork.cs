using Microsoft.Extensions.Logging;
using Payment.Api.Dal.IRepositories;
using Payment.Api.Dal.Repositories;
using Payment.Api.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PaymentDbContext _dbContext;
        public IPaymentRepository Payments { get; private set; }
        public IOrderRepository Orders { get; private set; }

        public UnitOfWork(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
            Payments = new PaymentRepository(dbContext);
            Orders = new OrderRepository(dbContext);
        }
        public UnitOfWork() { }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
