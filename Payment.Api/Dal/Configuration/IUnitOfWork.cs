using Payment.Api.Dal.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.Configuration
{
    public interface IUnitOfWork
    {
        IPaymentRepository Payments { get; }
        IOrderRepository Orders { get; }
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
