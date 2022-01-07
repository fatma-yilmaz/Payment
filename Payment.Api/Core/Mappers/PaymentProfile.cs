using AutoMapper;
using Payment.Api.Data.Queries;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Core.Mappers
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentEntity, GetPaymentByIdQueryResponse>();
        }
    }
}
