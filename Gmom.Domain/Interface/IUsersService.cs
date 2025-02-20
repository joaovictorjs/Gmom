namespace Gmom.Domain.Interface;

public interface IUsersService
{
    Task<bool> HasUserAdmin();
}