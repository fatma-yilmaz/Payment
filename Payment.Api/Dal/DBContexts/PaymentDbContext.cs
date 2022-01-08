using Microsoft.EntityFrameworkCore;
using Payment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.DBContexts
{
    public class PaymentDbContext :DbContext
    {
        public PaymentDbContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<PaymentEntity> Payments { get; set; }
    }
}
