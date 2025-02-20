using Gmom.Domain.Models;

namespace Gmom.Domain.Interface;

public interface IPostgresConnectionStore
{
     PostgresConnectionModel Value { get; set; } 
}