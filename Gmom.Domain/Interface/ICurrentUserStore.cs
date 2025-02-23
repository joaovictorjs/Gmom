using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface ICurrentUserStore
{
    UserModel Value { get; set; }
}