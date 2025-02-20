using Gmom.Domain.Interface;
using Gmom.Infrastructure.Stores;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Gmom.Infrastructure.Contexts;

public class PostgresContextFactory(IPostgresConnectionStore connectionStore)
    : IDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        var pgBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = connectionStore.Value.Host,
            Port = int.Parse(connectionStore.Value.Port),
            Database = connectionStore.Value.Database,
            Username = connectionStore.Value.Username,
            Password = connectionStore.Value.Password,
        };
        optionsBuilder.UseNpgsql(pgBuilder.ToString(), pg => pg.SetPostgresVersion(9, 5));
        return new PostgresContext(optionsBuilder.Options);
    }
}
