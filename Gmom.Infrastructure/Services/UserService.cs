using System.Security.Cryptography;
using System.Text;
using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Infrastructure.Services;

public class UserService(IRepository<UserEntity> repository) : IUserService
{
    public async Task<bool> HasAdmin()
    {
        return (await repository.WhereAsync(it => it.IsAdmin)).Any();
    }

    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Users);
    }

    public async Task<bool> Update(UserModel user, bool insert)
    {
        user.Password = toMD5(user.Password);

        if (insert)
        {
            return await repository.InsertAsync((UserEntity)user.ToEntity()) > 0;
        }

        return await repository.UpdateAsync((UserEntity)user.ToEntity()) > 0;
    }

    private string toMD5(string content)
    {
        var encoded = Encoding.UTF8.GetBytes(content);
        var hash = MD5.HashData(encoded);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
    }

    public async Task<bool> Delete(UserModel user)
    {
        return await repository.DeleteAsync((UserEntity)user.ToEntity()) > 0;
    }
}
