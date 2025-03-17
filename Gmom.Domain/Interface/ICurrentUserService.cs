using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface ICurrentUserService
{
    Task CheckIsAdmin();
    UserModel GetCurrentUser();
}