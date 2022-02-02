using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Services.Models
{
    public class Order
    {
        public string ConsumerFullName { get; set; }
        public string ConsumerAddress { get; set; }
    }
}
