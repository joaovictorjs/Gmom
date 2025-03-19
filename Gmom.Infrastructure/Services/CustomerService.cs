using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

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
}
