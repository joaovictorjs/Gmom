using System.Linq.Expressions;
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

public class Repository<T>(IDbContextFactory<PostgresContext> pgFactory) : IRepository<T>
    where T : class, IEntity
{
    public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        return await postgres.Set<T>().Where(predicate).ToListAsync();
    }

    public int NextValueForSequence(string sequenceName)
    {
        using var ctx = pgFactory.CreateDbContext();
        return ctx.Database.SqlQuery<int>($"SELECT NEXTVAL({sequenceName})").ToList().First();
    }

    public async Task<int> InsertAsync(T entity)
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        postgres.Set<T>().Add(entity);
        return await postgres.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        postgres.Set<T>().Update(entity);
        return await postgres.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(T entity)
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        postgres.Set<T>().Remove(entity);
        return await postgres.SaveChangesAsync();
    }

    public async Task<List<T>> ToList()
    {
        await using var postgres = await pgFactory.CreateDbContextAsync();
        return await postgres.Set<T>().ToListAsync();
    }
}