using Gmom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gmom.Infrastructure.Contexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(it => it.Name).IsUnique();

            entity.HasData(
                new UserEntity
                {
                    Id = 1,
                    Name = "ADMIN",
                    Password = "21232f297a57a5a743894a0e4a801fc3",
                    IsAdmin = true,
                }
            );
        });
    }
}
