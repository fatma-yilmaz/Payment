using Payment.Api.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Dal.IRepositories
{
    public interface IOrderRepository
    {
        Task<OrderEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Create(OrderEntity order, CancellationToken cancellationToken);
    }
}
