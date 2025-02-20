using Microsoft.EntityFrameworkCore;

namespace Gmom.Infrastructure.Contexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    
}