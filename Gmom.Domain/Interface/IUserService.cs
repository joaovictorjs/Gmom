using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface IUserService
{
    Task<bool> HasAdmin();
    int GetNextId();
    Task<bool> Update(UserModel user, bool insert);
    Task<bool> Delete(UserModel user);
}