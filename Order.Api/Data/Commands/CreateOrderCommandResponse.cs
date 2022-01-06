using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Data.Commands
{
    public class CreateOrderCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Guid OrderId { get; set; }
    }
}
