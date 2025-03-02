using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Domain.Entities;

[Table("produtos")]
public class ProductEntity : IEntity
{
    [Column("id"), Key]
    public required int Id { get; set; }
    [Column("nome")]
    public required string Name { get; set; }
    [Column("codigo_barras")]
    public required string BarCode { get; set; }
    [Column("preco")]
    public required double Price { get; set; }
    [Column("estoque")]
    public required double Stock { get; set; }

    public IModel ToModel()
    {
        return new ProductModel
        {
            Id = Id,
            Name = Name,
            BarCode = BarCode,
            Price = Price,
            Stock = Stock
        };
    }
}