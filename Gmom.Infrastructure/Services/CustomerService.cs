using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Infrastructure.Services;

public class CustomerService(IRepository<CustomerEntity> repository, ICurrentUserService currentUserService):ICustomerService
{
    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Customers);
    }
}