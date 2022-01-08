using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class CreatePaymentCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Guid PaymenId { get; set; }
    }
}
