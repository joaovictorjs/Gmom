using System.Diagnostics;
using Gmom.Domain.Constants;
using Gmom.Domain.Entities;
using Gmom.Domain.Enums;
using Gmom.Domain.Exceptions;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Services;

public class ProductService(
    IRepository<ProductEntity> repository,
    ICurrentUserService currentUserService
) : IProductService
{
    public int GetNextId()
    {
        return repository.NextValueForSequence(SequenceNames.Products);
    }

    public string GenerateBarCode()
    {
        var result = string.Empty;
        var random = new Random();
        for (var c = 0; c < 13; c++)
            result += random.Next(9);
        return result;
    }

    public async Task Save(ProductModel product, bool isUpdate)
    {
        await currentUserService.CheckIsAdmin();

        await CheckBarCode(product.BarCode, product.Id);

        if (isUpdate)
        {
            await repository.UpdateAsync((ProductEntity)product.ToEntity());
        }
        else
        {
            await repository.InsertAsync((ProductEntity)product.ToEntity());
        }
    }

    public async Task<List<ProductModel>> Find(string searchTerm, FindStrategy strategy)
    {
        searchTerm = searchTerm.Replace('\\', '%');

        var data = strategy switch
        {
            FindStrategy.Id => await repository.WhereAsync(it =>
                EF.Functions.ILike(it.Id.ToString(), searchTerm)
            ),
            FindStrategy.BarCode => await repository.WhereAsync(it =>
                EF.Functions.ILike(it.BarCode, searchTerm)
            ),
            FindStrategy.Name => await repository.WhereAsync(it =>
                EF.Functions.ILike(it.Name, searchTerm)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null),
        };

        return data.Select(it => (ProductModel)it.ToModel()).ToList();
    }

    private async Task CheckBarCode(string productBarCode, int productId)
    {
        var barCodeCheck = await repository.WhereAsync(it =>
            it.BarCode == productBarCode && it.Id != productId
        );

        if (barCodeCheck.Any())
        {
            throw new DuplicatedValueException(
                $"O código de barras {productBarCode} ja está sendo utilizado!"
            );
        }
    }
}
