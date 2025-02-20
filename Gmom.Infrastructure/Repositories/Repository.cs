using Gmom.Domain.Interface;
using Gmom.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Repositories;

public class Repository(IDbContextFactory<PostgresContext> pgFactory) : IRepository
{
    public async Task MigrateAsync()
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        await postgres.Database.MigrateAsync();
    }
}