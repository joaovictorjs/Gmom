using Gmom.Domain.Enums;
using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface ICustomerService
{
    int GetNextId();
    Task Save(CustomerModel customer, bool isUpdate);
    Task<List<CustomerModel>> Find(string searchTerm, FindStrategy strategy);
    Task Delete(CustomerModel customer);
}