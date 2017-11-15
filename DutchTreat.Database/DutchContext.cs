using System;
using Microsoft.EntityFrameworkCore;
using DutchTreat.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DutchTreat.Database
{
    //pointers for how to get the database going in docker
    //https://docs.microsoft.com/en-gb/sql/linux/sql-server-linux-setup-tools
    //https://docs.microsoft.com/en-gb/sql/linux/quickstart-install-connect-docker
    //docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd12' -p 1433:1433 -d microsoft/mssql-server-linux
    public class DutchContext:IdentityDbContext<DutchUser>//DbContext//IdentityDbContext<DutchUser>
    {
        public DbSet<Product> Products {get;set;}
        public DbSet<Order> Orders {get;set;}

        public DutchContext(DbContextOptions<DutchContext> options):base(options)
        {
            //Database.EnsureCreated();
        }

        

        
    }
}
