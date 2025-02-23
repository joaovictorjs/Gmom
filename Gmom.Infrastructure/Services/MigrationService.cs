using Gmom.Domain.Interface;

namespace Gmom.Infrastructure.Services;

public class MigrationService(IRepository repository) : IMigrationService
{
    public async Task MigrateAsync()
    {
        await repository.MigrateAsync();
    }
}