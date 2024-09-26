using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests;

[TestClass]
public class UserRepositoryTest
{
    private HMDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<HMDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HMDbContext(options);
    }

    private void SeedData(HMDbContext context)
    {
        var permission = new PermissionKey { Id = Guid.NewGuid(), Value = "ExamplePermission" };

        context.PermissionKeys?.Add(permission);

        var adminRole = new Role
        {
            Id = Guid.NewGuid(), Name = "Admin", Permissions = new List<PermissionKey> { permission }
        };

        var homeownerRole = new Role
        {
            Id = Guid.NewGuid(), Name = "HomeOwner", Permissions = new List<PermissionKey> { permission }
        };

        var companyOwnerRole = new Role
        {
            Id = Guid.NewGuid(), Name = "CompanyOwner", Permissions = new List<PermissionKey> { permission }
        };

        context.Roles?.AddRange(adminRole, homeownerRole, companyOwnerRole);

        context.SaveChanges();
    }

    [TestMethod]
    public void CreateAdminTest()
    {
        using (var context = CreateInMemoryDbContext("TestAddAdmin"))
        {
            SeedData(context); // Llamar al método para hacer el seed

            var repository = new UserRepository(context);
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123"
            };

            // Llama al método que crea el admin y asigna el rol correspondiente
            var result = repository.CreateAdmin(expected);
            context.SaveChanges();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Email, result.Email);

            var storedAdmin = context.Users?.FirstOrDefault(a => a.Id == expected.Id);
            Assert.IsNotNull(storedAdmin);
            Assert.AreEqual(expected.Email, storedAdmin.Email);

            Assert.IsNotNull(storedAdmin.Role);
            Assert.AreEqual("Admin", storedAdmin.Role.Name, "El usuario debe tener el rol Admin asignado.");
        }
    }

    [TestMethod]
    public void GetUsersTest()
    {
        using (var context = CreateInMemoryDbContext("TestGetUsers"))
        {
            var repository = new UserRepository(context);

            var expected = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Juan",
                    LastName = "Perez",
                    Email = "mail@mail.com",
                    Password = "securePassword123"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jose",
                    LastName = "Gervasio",
                    Email = "mail@mail.com",
                    Password = "securePassword123"
                }
            };

            context.Users?.AddRange(expected);
            context.SaveChanges();

            var result = repository.GetUsers();

            Assert.AreEqual(expected.Count, result.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, result[i].Id);
                Assert.AreEqual(expected[i].Email, result[i].Email);
            }

        }
    }

    [TestMethod]
    public void CreateCompanyOwnerTest()
    {
        using (var context = CreateInMemoryDbContext("TestAddCompanyOwner"))
        {
            SeedData(context);

            var repository = new UserRepository(context);
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123"
            };

            var result = repository.CreateCompanyOwner(expected);
            context.SaveChanges();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Email, result.Email);

            Assert.IsNotNull(result.Role);
            Assert.AreEqual("CompanyOwner", result.Role.Name, "El usuario debe tener el rol CompanyOwner asignado.");
        }

    }

    [TestMethod]
    public void CreateHomeOwnerTest()
    {
        using (var context = CreateInMemoryDbContext("TestAddCompanyOwner"))
        {
            SeedData(context);

            var repository = new UserRepository(context);
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123"
            };

            var result = repository.CreateHomeOwner(expected);
            context.SaveChanges();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Email, result.Email);

            Assert.IsNotNull(result.Role);
            Assert.AreEqual("HomeOwner", result.Role.Name, "El usuario debe tener el rol HomeOwner asignado.");
        }
    }

    [TestMethod]
    public void AddCompanyToCompanyOwnerTest()
    {
        using (var context = CreateInMemoryDbContext("TestAddCompanyToCompanyOwner"))
        {
            SeedData(context);

            var companyOwnerRole = context.Roles?.FirstOrDefault(r => r.Name == "CompanyOwner");

            var repository = new UserRepository(context);
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123",
                Role = companyOwnerRole,
            };

            var company = new Company { Id = Guid.NewGuid(), Name = "Company" };

            var result = repository.AddCompanyToCompanyOwner(expected, company);
            context.SaveChanges();
            
            Assert.IsNotNull(result.Company);
            Assert.AreEqual(company.Id, result.Company?.Id);
            Assert.AreEqual(company.Name, result.Company?.Name);
        }
    }
}
