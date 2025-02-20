namespace Gmom.Domain.Interface;

public interface IUserService
{
    Task<bool> HasUserAdmin();
}