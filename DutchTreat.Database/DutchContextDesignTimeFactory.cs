using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DutchTreat.Database{

    public class DutchContextDesignTimeFactory : IDesignTimeDbContextFactory<DutchContext>
    {
        DutchContext IDesignTimeDbContextFactory<DutchContext>.CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DutchContext>();
            builder.UseSqlServer(
                "Server=localhost;Database=DutchTreat;User ID=SA;Password=;MultipleActiveResultSets=true;");

            return new DutchContext(builder.Options);        }
    }

}
