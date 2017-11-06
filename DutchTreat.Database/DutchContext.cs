using System;
using Microsoft.EntityFrameworkCore;
using DutchTreat.Database.Entities;

namespace DutchTreat.Database
{
    public class DutchContext:DbContext
    {
        public DbSet<Product> Products {get;set;}
        public DbSet<Order> Orders {get;set;}

        public DutchContext(DbContextOptions<DutchContext> options):base(options)
        {
            
        }
    }
}
