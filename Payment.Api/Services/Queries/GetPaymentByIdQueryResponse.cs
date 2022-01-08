using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Queries
{
    public class GetPaymentByIdQueryResponse
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        public OrderDto Order { get; set; }
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string ConsumerFullName { get; set; }
        public string ConsumerAddress { get; set; }
    }
}
