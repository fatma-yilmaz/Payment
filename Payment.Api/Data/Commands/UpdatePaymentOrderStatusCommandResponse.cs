using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Data.Commands
{
    public class UpdatePaymentOrderStatusCommandResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
