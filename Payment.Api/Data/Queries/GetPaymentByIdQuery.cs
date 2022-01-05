using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Queries
{
    public class GetPaymentByIdQuery :IRequest<GetPaymentByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
