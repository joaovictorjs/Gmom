using System.ComponentModel.DataAnnotations.Schema;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Domain.Entities;

[Table("clientes")]
public class CustomerEntity:IEntity
{
    public required int Id {get;set;}
    public required string Name {get;set;}
    
    public IModel ToModel()
    {
        return new CustomerModel
        {
            Id = Id,
            Name = Name
        };
    }
}