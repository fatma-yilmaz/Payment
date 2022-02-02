using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Payment.Api.Core.Exceptions
{
    public class PaymentException:Exception
    {
        public readonly HttpStatusCode StatusCode;

        public PaymentException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
