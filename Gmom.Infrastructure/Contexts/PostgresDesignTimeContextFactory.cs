using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gmom.Infrastructure.Contexts;

public class PostgresDesignTimeContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        optionsBuilder.UseNpgsql(string.Empty, pg => pg.SetPostgresVersion(9, 5));
        return new PostgresContext(optionsBuilder.Options);
    }
}
