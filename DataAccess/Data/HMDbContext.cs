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

    public DbSet<Device>? Devices { get; set; }
    public DbSet<HomeDevice>? HomeDevices { get; set; }
    public DbSet<Company>? Companies { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<Home>? Homes { get; set; }
    public DbSet<MemberSetting>? MemberSettings { get; set; }
    public DbSet<Permission>? Permissions { get; set; }
    public DbSet<PermissionKey>? PermissionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed de permisos
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = Guid.Parse("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), Value = "CanAsociateDevices" },
            new Permission { Id = Guid.Parse("4d99af50-c4b9-4bc7-8c63-6e4f6f24a73a"), Value = "CanListDevices" },
            new Permission { Id = Guid.Parse("c0f0d7a7-3e77-4128-87d3-30113b19936d"), Value = "CanGetNotifications" },
            new Permission { Id = Guid.Parse("b2ff8154-fdb0-4a87-a7b5-ded13fb66f57"), Value = "CanAddMembers" }
        );

        modelBuilder.Entity<PermissionKey>().HasData(
            new PermissionKey { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Value = "CanCreateAdmin" },
            new PermissionKey { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Value = "CanCreateCompanyOwner" },
            new PermissionKey { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Value = "CanCreateHomeOwner" },
            new PermissionKey { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62"), Value = "CanCreateCompany" }
        );


        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Name = "Admin" },
            new Role { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Name = "HomeOwner" },
            new Role { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Name = "CompanyOwner" }
        );

        // Configuraciones de relación muchos a muchos sin entidad intermedia
        modelBuilder.Entity<MemberSetting>()
            .HasMany(ms => ms.Permissions)
            .WithMany(p => p.MemberSettings)
            .UsingEntity(j => j.ToTable("MemberSettingPermissions"));



        // Agregar datos en la tabla de relación (RolePermissions)
        modelBuilder.Entity<Role>()
            .HasMany(r => r.PermissionKeys)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.HasData(
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74") }, // Admin -> CanCreateAdmin
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf") }, // Admin -> CanCreateCompanyOwner
                new { RolesId = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), PermissionKeysId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61") },  // HomeOwner -> CanCreateHomeOwner
                new { RolesId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), PermissionKeysId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62") } // CompanyOwner -> CanCreateCompany
                ));

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleID);

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
