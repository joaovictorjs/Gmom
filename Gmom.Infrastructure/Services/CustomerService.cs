using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Enums;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Services;

public class CustomerService(
    IRepository<CustomerEntity> repository,
    ICurrentUserService currentUserService
) : ICustomerService
{
    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Customers);
    }

    public async Task Save(CustomerModel customer, bool isUpdate)
    {
        await currentUserService.CheckIsAdmin();

        if (isUpdate)
        {
            await repository.UpdateAsync((CustomerEntity)customer.ToEntity());
        }
        else
        {
            await repository.InsertAsync((CustomerEntity)customer.ToEntity());
        }
    }

    public async Task<List<CustomerModel>> Find(string searchTerm, FindStrategy strategy)
    {
        searchTerm = searchTerm.Replace('\\', '%');

        var data = strategy switch
        {
            FindStrategy.Id => await repository.WhereAsync(it =>
                EF.Functions.ILike(it.Id.ToString(), searchTerm)
            ),
            FindStrategy.Name => await repository.WhereAsync(it =>
                EF.Functions.ILike(it.Name, searchTerm)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null),
        };

        return data.Select(it => (CustomerModel)it.ToModel()).ToList();
    }
}
