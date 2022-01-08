using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Data.Queries
{
    public class GetOrderByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string ConsumerFullName { get; set; }
        public string ConsumerAddress { get; set; }
    }
}
