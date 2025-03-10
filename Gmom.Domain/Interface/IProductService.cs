using Gmom.Domain.Enums;
using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface IProductService
{
    int GetNextId();
    string GenerateBarCode();
    Task Save(ProductModel product, bool isUpdate);
    Task<List<ProductModel>> Find(string searchTerm, FindStrategy strategy);
    Task Delete(ProductModel product);
}