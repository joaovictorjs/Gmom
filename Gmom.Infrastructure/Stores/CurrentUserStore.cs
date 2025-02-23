using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Infrastructure.Stores;

public class CurrentUserStore : ICurrentUserStore
{
    public UserModel Value { get; set; } = new UserModel
    {
        Id = 0,
        Name = string.Empty,
        Password = string.Empty,
        IsAdmin = false
    };
}