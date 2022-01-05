﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Entities
{
    public class PaymentEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        //public int OrderId { get; set; }
    }
}
