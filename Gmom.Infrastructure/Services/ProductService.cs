using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Infrastructure.Services;

public class ProductService(IRepository<ProductEntity> repository) : IProductService
{
    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Products);
    }
}