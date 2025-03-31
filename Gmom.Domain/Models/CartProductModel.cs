using Gmom.Domain.Interface;

namespace Gmom.Domain.Models;

public class CartProductModel : IModel
{ 
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string BarCode { get; set; }
    public required double Price { get; set; }
    public required double Total { get; set; }
    public required double Quantity { get; set; }
    public IEntity ToEntity()
    {
        throw new NotImplementedException();
    }
}