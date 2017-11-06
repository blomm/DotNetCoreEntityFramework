using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Api.Database
{
    public class CampContextFactory : IDesignTimeDbContextFactory<CampContext>
    {

        CampContext IDesignTimeDbContextFactory<CampContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CampContext>();
            builder.UseSqlServer(
                "Server=localhost;Database=CampDemo;User ID=SA;Password=P@ssw0rd12;MultipleActiveResultSets=true;");

            return new CampContext(builder.Options);
        }
    }
}