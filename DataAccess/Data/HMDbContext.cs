using System.Diagnostics.CodeAnalysis;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

[ExcludeFromCodeCoverage]
public class HMDbContext : DbContext
{
    public HMDbContext(DbContextOptions<HMDbContext> options) : base(options)
    {
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<Home>? Homes { get; set; }
    public DbSet<PermissionKey>? PermissionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Name = "Admin" },
            new Role { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Name = "HomeOwner" },
            new Role { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Name = "CompanyOwner" }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                Name = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                Password = "admin",
                Role = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74")
            });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=API;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
                );
        }
    }
}
