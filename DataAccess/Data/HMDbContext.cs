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
        modelBuilder.Entity<Device>()
            .HasDiscriminator<DeviceType>("DeviceType")
            .HasValue<Device>(DeviceType.Sensor)
            .HasValue<Camera>(DeviceType.Camera);

        modelBuilder.Entity<Device>()
            .HasOne(d => d.Company)
            .WithMany()
            .HasForeignKey(d => d.CompanyId);


        var userId = Guid.Parse("205e7ec9-673c-4db2-911d-10fe2b9c159a");
        var companyId = Guid.Parse("10570280-239e-4fb8-8939-4f37415fccb7");

        var user = new User
        {
            Id = userId,
            Name = "anotherCompanyOwner",
            LastName = "anotherCompanyOwner",
            Email = "anothercompanyowner1@gmail.com",
            Password = "companyowner@1",
            RoleID = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"),
            CompanyID = companyId // Set the foreign key for the company
        };
        modelBuilder.Entity<User>().HasData(user);

        var company = new Company
        {
            Id = companyId,
            Name = "Samsung",
            RUT = "2141412",
            Logo = "sadas/dasdasdas/asdasd",
            OwnerId = userId // Set the foreign key for the owner
        };

        modelBuilder.Entity<Company>()
            .HasData(company);

        //Seed de Devices
        modelBuilder.Entity<Device>().HasData(
            new Device { Id = Guid.Parse("5d95af52-c4b9-4bc7-8c63-6e4f6f24a73a"), Name = "Lampara", Model = "Modelo 1", CompanyId = companyId, DeviceType = DeviceType.Sensor, Description = "Lampara de techo", Photo = "https://www.google.com" },
            new Device { Id = Guid.Parse("6d95af53-c4b9-4bc7-8c63-6e4f6f24a73a"), Name = "Lampara de avion", Model = "Modelo 2", DeviceType = DeviceType.Sensor, CompanyId = companyId, Description = "Lampara de avion", Photo = "https://www.avion.com" }
        );

        // Seed de permisos
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = Guid.Parse("7fa6a0f4-d7d9-4c89-a85e-92b937fc0274"), Value = "CanAsociateDevices" },
            new Permission { Id = Guid.Parse("4d99af50-c4b9-4bc7-8c63-6e4f6f24a73a"), Value = "CanListDevices" },
            new Permission { Id = Guid.Parse("c0f0d7a7-3e77-4128-87d3-30113b19936d"), Value = "CanGetNotifications" },
            new Permission { Id = Guid.Parse("b2ff8154-fdb0-4a87-a7b5-ded13fb66f57"), Value = "CanAddMembers" }
        );

        //Seed de PermissionKeys
        modelBuilder.Entity<PermissionKey>().HasData(
            new PermissionKey { Id = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), Value = "CanCreateAdmin" },
            new PermissionKey { Id = Guid.Parse("e43167ad-158b-4a39-8f5d-c0a69b32d7cf"), Value = "CanCreateCompanyOwner" },
            new PermissionKey { Id = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), Value = "CanCreateHomeOwner" },
            new PermissionKey { Id = Guid.Parse("8aed0b92-ab5b-47f3-af36-220ab60b66e4"), Value = "CanCreateCompany" },
            new PermissionKey { Id = Guid.Parse("265ab018-9afa-4f99-acaa-7d9082bfe5ad"), Value = "CanCreateADevice" },
            new PermissionKey { Id = Guid.Parse("a43167ad-158b-5a38-8f5d-c1a69b32d7cf"), Value = "CanManageUsers" }
        );

        //Seed de Roles
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
                new { RolesId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), PermissionKeysId = Guid.Parse("8aed0b92-ab5b-47f3-af36-220ab60b66e4") }, // CompanyOwner -> CanCreateCompany
                new { RolesId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a61"), PermissionKeysId = Guid.Parse("265ab018-9afa-4f99-acaa-7d9082bfe5ad") }, //CompanyOwner -> CanCreateDevices
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("a43167ad-158b-5a38-8f5d-c1a69b32d7cf") }, // Admin -> CanManageUsers
                new { RolesId = Guid.Parse("b4a6e6cd-856e-4ad1-a87e-9f1b24d40a74"), PermissionKeysId = Guid.Parse("c9a4a8f5-4393-4b5e-8c57-e7f5f58a9a62") } // Admin -> CanCreateCompany
                ));

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleID);

        modelBuilder.Entity<User>().HasOne(u => u.Company)
            .WithOne(c => c.Owner)
            .HasForeignKey<Company>(c => c.OwnerId);


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
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
