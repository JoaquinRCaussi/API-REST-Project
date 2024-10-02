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
        modelBuilder.Entity<PermissionKey>().HasData(
            new PermissionKey { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Value = "CanCreateAdmin" },
            new PermissionKey { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Value = "CanCreateCompanyOwner" },
            new PermissionKey { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Value = "CanCreateHomeOwner" }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Name = "Admin" },
            new Role { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Name = "HomeOwner" },
            new Role { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Name = "CompanyOwner" }
        );

        // Agregar datos en la tabla de relación (RolePermissions)
        modelBuilder.Entity<Role>()
            .HasMany(r => r.PermissionKeys)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.HasData(
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") }, // Admin -> CanCreateAdmin
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") }, // Admin -> CanCreateCompanyOwner
                new { RolesId = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), PermissionKeysId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") }  // HomeOwner -> CanCreateHomeOwner
            ));

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                Name = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                Password = "admin",
                RoleID = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74")
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                Name = "HomeOwner",
                LastName = "HomeOwner",
                Email = "homeowner1@gmail.com",
                Password = "homeowner@1",
                RoleID = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf")
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
                Name = "CompanyOwner",
                LastName = "CompanyOwner",
                Email = "companyowner1@gmail.com",
                Password = "companyowner@1",
                RoleID = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61")
            }
        );
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
