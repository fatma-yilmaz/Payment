using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.IRepositories
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Create(PaymentEntity payment, CancellationToken cancellationToken);
        Task Update(PaymentEntity payment, CancellationToken cancellationToken);
    }
}
