using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Infrastructure.Stores;

public class PostgresConnectionStore : IPostgresConnectionStore
{
    public PostgresConnectionModel Value { get; set; } =
        new()
        {
            Host = string.Empty,
            Port = string.Empty,
            Database = string.Empty,
            Username = string.Empty,
            Password = string.Empty,
        };
}
