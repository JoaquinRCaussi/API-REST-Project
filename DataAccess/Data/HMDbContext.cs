using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

public class HMDbContext : DbContext
{
    public HMDbContext(DbContextOptions<HMDbContext> options) : base(options)
    {
        
    }
    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<PermissionKey>? PermissionKeys { get; set; }
}
