using AutoMapper;
using Payment.Api.Data.HttpClients;
using Payment.Api.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Core.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile() {
            CreateMap<GetOrderResponse, OrderDto>()
                .ForMember(destination => destination.Id, operation => operation.MapFrom(source => source.id))
                .ForMember(destination => destination.ConsumerFullName, operation => operation.MapFrom(source => source.consumerFullName))
                .ForMember(destination => destination.ConsumerAddress, operation => operation.MapFrom(source => source.consumerAddress));
        }
    }
}
