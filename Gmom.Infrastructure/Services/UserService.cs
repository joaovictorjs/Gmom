using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Infrastructure.Services;

public class UserService(IRepository<UserEntity> repository) : IUserService
{
    public async Task<bool> HasUserAdmin()
    {
        return (await repository.WhereAsync(it => it.IsAdmin)).Any();
    }
}
