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

    [TestMethod]
    public void CreateAdminTest()
    {
        using (var context = CreateInMemoryDbContext("TestAddAdmin"))
        {
            var repository = new UserRepository(context);
            var expected = new User
            {
                Id = Guid.NewGuid(),
                Name = "Juan",
                LastName = "Perez",
                Email = "mail@mail.com",
                Password = "securePassword123"
            };

            var result = repository.CreateAdmin(expected);
            context.SaveChanges();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Email, result.Email);

            var storedAdmin = context.Users?.FirstOrDefault(a => a.Id == expected.Id);
            Assert.IsNotNull(storedAdmin);
            Assert.AreEqual(expected.Email, storedAdmin.Email);
        }
    }

}
