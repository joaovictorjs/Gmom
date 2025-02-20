using Gmom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Contexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; }
}
