using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Services.Models
{
    public class CreatePaymentServiceRequest
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public Order Order { get; set; }
    }
}
