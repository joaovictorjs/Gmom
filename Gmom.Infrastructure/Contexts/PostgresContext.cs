using Gmom.Domain.Entities;
using Gmom.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Contexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; }
    public DbSet<ProductEntity> Products { get; }
    public DbSet<CustomerEntity> Customers { get; }

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

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasIndex(it => it.BarCode).IsUnique();
        });

        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasData(new CustomerEntity { Id = 1, Name = "CONSUMIDOR FINAL" });
        });
    }
}
