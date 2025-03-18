using Gmom.Domain.Entities;
using Gmom.Domain.Interface;

namespace Gmom.Domain.Models;

public class CustomerModel:IModel
{
    public required int Id {get;set;}
    public required string Name {get;set;}
    
    public IEntity ToEntity()
    {
        return new CustomerEntity
        {
            Id = Id,
            Name = Name
        };
    }
}