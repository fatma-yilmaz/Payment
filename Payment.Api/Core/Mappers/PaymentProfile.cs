using AutoMapper;
using Payment.Api.Core.Enums;
using Payment.Api.Entities;
using Payment.Api.Services.Models;
using System;

namespace Payment.Api.Core.Mappers
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentEntity, GetPaymentByIdServiceResponse>()
                .ForMember(destination => destination.Status,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(PaymentStatus), source.Status)));
        }
    }
}
