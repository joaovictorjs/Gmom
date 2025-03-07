namespace Gmom.Domain.Interface;

public interface ICurrentUserService
{
    Task CheckIsAdmin();
}