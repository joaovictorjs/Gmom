using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface ICustomerService
{
    int GetNextId();
    Task Save(CustomerModel customer, bool isUpdate);
}