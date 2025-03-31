namespace Gmom.Domain.Models;

public class CartModel
{ 
    public required string Name { get; set; }
    public required string BarCode { get; set; }
    public required double Price { get; set; }
    public required double Quantity { get; set; }
}