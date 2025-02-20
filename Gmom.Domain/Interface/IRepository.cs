using System.Linq.Expressions;

namespace Gmom.Domain.Interface;

public interface IRepository
{
    Task MigrateAsync();
}

public interface IRepository<T>  where T : IEntity
{
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}