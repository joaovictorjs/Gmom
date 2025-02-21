using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Domain.Models;

public class UserModel : IModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required bool IsAdmin { get; set; }

    public IEntity ToEntity()
    {
        return new UserEntity
        {
            Id = Id,
            Name = Name,
            Password = Password,
            IsAdmin = IsAdmin,
        };
    }
}
