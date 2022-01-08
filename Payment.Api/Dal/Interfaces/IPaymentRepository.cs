using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Dal.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity> GetById(Guid id);
        Task<Guid> Create(PaymentEntity payment);
        Task<bool> Update(PaymentEntity payment);
    }
}
