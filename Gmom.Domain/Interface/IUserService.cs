using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface IUserService
{
    int GetNextId();
    Task<bool> Update(UserModel user, bool insert);
    Task<bool> Delete(UserModel user);
    Task<UserModel> Login(string name, string password);
    Task<List<UserModel>> GetAll();
    Task Save(UserModel newer, UserModel? older);
}