using System.Diagnostics.CodeAnalysis;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Data;

[ExcludeFromCodeCoverage]
public class HMDbContext : DbContext
{
    public HMDbContext(DbContextOptions<HMDbContext> options) : base(options)
    {
    }

    public DbSet<Notification>? Notifications { get; set; }
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
        // Definir GUIDs manualmente
        var userId = Guid.Parse("d84722d7-8b0a-4ae6-aedd-111111111111");
        var companyId = Guid.Parse("8b02a6f7-6a7e-45c8-899e-222222222222");
        var adminRoleId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61");
        var homeOwnerRoleId = Guid.Parse("6d72b33a-582b-411e-a9b1-333333333333");
        var companyOwnerRoleId = Guid.Parse("78947c68-f0aa-49d3-8f47-444444444444");

        // Datos semilla para Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "Admin" },
            new Role { Id = homeOwnerRoleId, Name = "HomeOwner" },
            new Role { Id = companyOwnerRoleId, Name = "CompanyOwner" }
        );

        // Datos semilla para Permissions
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = Guid.Parse("1a6b8ddf-3f92-4fda-87a4-777777777777"), Value = "CanAsociateDevices" },
            new Permission { Id = Guid.Parse("d47fa8f6-ace7-42e5-8bdf-888888888888"), Value = "CanListDevices" },
            new Permission { Id = Guid.Parse("2ebd4f21-3cd4-431f-97e7-999999999999"), Value = "CanGetNotifications" },
            new Permission { Id = Guid.Parse("3bcde1b8-5ad2-4f6c-92c7-101010101010"), Value = "CanAddMembers" }
        );

        // Datos semilla para PermissionKeys
        modelBuilder.Entity<PermissionKey>().HasData(
            new PermissionKey { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Value = "CanCreateAdmin" },
            new PermissionKey { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Value = "CanCreateCompanyOwner" },
            new PermissionKey { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Value = "CanCreateHomeOwner" },
            new PermissionKey { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Value = "CanCreateCompany" },
            new PermissionKey { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Value = "CanCreateADevice" },
            new PermissionKey { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Value = "CanManageUsers" }
        );

        // Configuración de relaciones many-to-many entre Roles y PermissionKeys
        modelBuilder.Entity<Role>()
            .HasMany(r => r.PermissionKeys)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.HasData(
                new { RolesId = adminRoleId, PermissionKeysId = Guid.Parse("11111111-1111-1111-1111-111111111111") },        // Admin -> CanCreateAdmin
                new { RolesId = adminRoleId, PermissionKeysId = Guid.Parse("66666666-6666-6666-6666-666666666666") },        // Admin -> CanManageUsers
                new { RolesId = adminRoleId, PermissionKeysId = Guid.Parse("22222222-2222-2222-2222-222222222222") },        // Admin -> CanCreateCompanyOwner
                new { RolesId = companyOwnerRoleId, PermissionKeysId = Guid.Parse("44444444-4444-4444-4444-444444444444") }, // CompanyOwner -> CanCreateCompanyOwner
                new { RolesId = companyOwnerRoleId, PermissionKeysId = Guid.Parse("55555555-5555-5555-5555-555555555555") }, // CompanyOwner -> CanCreateADevice
                new { RolesId = homeOwnerRoleId, PermissionKeysId = Guid.Parse("33333333-3333-3333-3333-333333333333") }     // HomeOwner -> CanCreateHomeOwner
            ));

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleID);

        modelBuilder.Entity<User>().HasOne(u => u.Company)
            .WithOne(c => c.Owner)
            .HasForeignKey<Company>(c => c.OwnerId);

        modelBuilder.Entity<Device>()
            .HasOne(d => d.Company)
            .WithMany()
            .HasForeignKey(d => d.CompanyId);



        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"),
                Name = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                Password = "admin",
                RoleID = adminRoleId
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"),
                Name = "HomeOwner",
                LastName = "HomeOwner",
                Email = "homeowner1@gmail.com",
                Password = "homeowner@1",
                RoleID = homeOwnerRoleId
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
                RoleID = companyOwnerRoleId
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
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
