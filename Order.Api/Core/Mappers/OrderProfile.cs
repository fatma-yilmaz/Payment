using AutoMapper;
using Order.Api.Data.Queries;
using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Core.Mappers
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderEntity, GetOrderByIdQueryResponse>()
                .ForMember(destination => destination.Id, operation => operation.MapFrom(source => source.Id))
                .ForMember(destination => destination.ConsumerFullName, operation => operation.MapFrom(source => source.ConsumerFullName))
                .ForMember(destination => destination.ConsumerAddress, operation => operation.MapFrom(source => source.ConsumerAddress));
        }
    }
}
