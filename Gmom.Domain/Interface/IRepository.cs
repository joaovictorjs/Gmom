using System.Linq.Expressions;

namespace Gmom.Domain.Interface;

public interface IRepository
{
    Task MigrateAsync();
}

public interface IRepository<T> where T : IEntity
{
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    int NextValueForSequence(string sequenceName);
    Task<int> InsertAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<List<T>> ToList();
}