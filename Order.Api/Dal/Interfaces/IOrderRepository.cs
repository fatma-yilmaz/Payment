using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Dal.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderEntity> GetById(Guid id);
        Task<Guid> Create(OrderEntity order);
    }
}
