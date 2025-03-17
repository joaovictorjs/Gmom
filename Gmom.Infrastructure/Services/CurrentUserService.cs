using Gmom.Domain.Entities;
using Gmom.Domain.Exceptions;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Infrastructure.Services;

public class CurrentUserService(ICurrentUserStore store, IRepository<UserEntity> repository)
    : ICurrentUserService
{
    public async Task CheckIsAdmin()
    {
        if (!(await repository.WhereAsync(it => it.Id == store.Value.Id && it.IsAdmin)).Any())
        {
            throw new AdminException("É necessário ser administrador para realizar esta ação!");
        }
    }

    public UserModel GetCurrentUser()
    {
        return store.Value;
    }
}
