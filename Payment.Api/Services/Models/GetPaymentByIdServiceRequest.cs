using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Services.Models
{
    public class GetPaymentByIdServiceRequest
    {
        public Guid Id { get; set; }
    }
}
