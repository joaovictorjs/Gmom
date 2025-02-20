namespace Gmom.Domain.Interface;

public interface IRepository
{
    Task MigrateAsync();
}