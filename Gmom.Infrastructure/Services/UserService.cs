﻿using System.Security.Cryptography;
using System.Text;
using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Exceptions;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using InvalidOperationException = System.InvalidOperationException;

namespace Gmom.Infrastructure.Services;

public class UserService(IRepository<UserEntity> repository, ICurrentUserService currentUserService)
    : IUserService
{
    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Users);
    }

    public async Task<bool> Update(UserModel user, bool insert)
    {
        user.Password = toMD5(user.Password);

        if (insert)
            return await repository.InsertAsync((UserEntity)user.ToEntity()) > 0;

        return await repository.UpdateAsync((UserEntity)user.ToEntity()) > 0;
    }

    public async Task<bool> Delete(UserModel user)
    {
        await currentUserService.CheckIsAdmin();

        if (currentUserService.GetCurrentUser().Id == user.Id)
        {
            throw new CurrentUserDeleteException($"Não é possível deletar o usuário '{user.Name}', pois ele está atualmente em uso para sua sessão!");
        }
        
        return await repository.DeleteAsync((UserEntity)user.ToEntity()) > 0;
    }

    public async Task<UserModel> Login(string name, string password)
    {
        var user = (await repository.WhereAsync(it => it.Name == name)).FirstOrDefault();

        if (user == null)
            throw new InvalidOperationException($"O usuário {name} não foi encontrado!");

        if (toMD5(password) != user.Password)
            throw new InvalidOperationException("Senha incorreta!");

        return (UserModel)user.ToModel();
    }

    public async Task<List<UserModel>> GetAll()
    {
        return (await repository.ToList()).Select(it => (UserModel)it.ToModel()).ToList();
    }

    public async Task Save(UserModel newer, UserModel? older)
    {
        await currentUserService.CheckIsAdmin();

        await CheckName(newer.Name, newer.Id);

        if (older != null)
        {
            newer.Password = string.IsNullOrWhiteSpace(newer.Password)
                ? older.Password
                : toMD5(newer.Password);

            await repository.UpdateAsync((UserEntity)newer.ToEntity());
        }
        else
        {
            newer.Password = toMD5(newer.Password);
            await repository.InsertAsync((UserEntity)newer.ToEntity());
        }
    }

    private async Task CheckName(string name, int id)
    {
        var userCheck = await repository.WhereAsync(it => it.Name == name && it.Id != id);

        if (userCheck.Any())
        {
            throw new DuplicatedValueException($"O nome {name} ja está sendo utilizado!");
        }
    }

    private string toMD5(string content)
    {
        var encoded = Encoding.UTF8.GetBytes(content);
        var hash = MD5.HashData(encoded);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
    }
}
