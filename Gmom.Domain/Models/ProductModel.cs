using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Domain.Models;

public class ProductModel :IModel
{   public required int Id { get; set; }
    public required string Name { get; set; }
    public required string BarCode { get; set; }
    public required double Price { get; set; }
    public required double Stock { get; set; }
    
    public IEntity ToEntity()
    {
        return new ProductEntity
        {
            Id = Id,
            Name = Name,
            BarCode = BarCode,
            Price = Price,
            Stock = Stock
        };
    }
}