using Payment.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Api.Services
{
    public interface IPaymentService
    {
        Task<CreatePaymentServiceResponse> Create(CreatePaymentServiceRequest request, CancellationToken cancellationToken);
        Task<GetPaymentByIdServiceResponse> GetById(GetPaymentByIdServiceRequest request, CancellationToken cancellationToken);
    }
}
