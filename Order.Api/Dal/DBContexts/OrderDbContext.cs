using Microsoft.EntityFrameworkCore;
using Order.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.DBContexts
{
    public class OrderDbContext :DbContext
    {
        public OrderDbContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<OrderEntity> Orders { get; set; }
    }
}
