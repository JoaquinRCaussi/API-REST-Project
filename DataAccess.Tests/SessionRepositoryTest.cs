using DataAccess.Data;
using DataAccess.Repositories;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DataAccess.Tests;

[TestClass]
public class SessionRepositoryTest
{
    private Mock<ISessionRepository>? _sessionRepositoryMock;
    
    [TestInitialize]
    public void Initialize()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
    }
    
    private HMDbContext CreateInMemoryDbContext(string dbName)
    {
        DbContextOptions<HMDbContext>? options = new DbContextOptionsBuilder<HMDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new HMDbContext(options);
    }
    
    private void SeedData(HMDbContext context)
    {
        var session = new Session { RoleID = Guid.NewGuid(), Token = Guid.NewGuid(), UserID = Guid.NewGuid() };
        _sessionRepositoryMock.Setup(x => x.AddSession(session));
    }

    [TestMethod]
    public void AddSessionTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestFindByToken");
        SeedData(context);
        
        var expected = new Session
        {
            RoleID = Guid.NewGuid(),
            Token = Guid.NewGuid(),
            UserID = Guid.NewGuid()
        };

        var repository = new SessionRepository(context);
        
        repository.AddSession(expected);
        
        var result = repository.FindByToken(expected.Token);
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Token, result.Token);
    }
    
    [TestMethod]
    public void FindByTokenTest()
    {
        using HMDbContext? context = CreateInMemoryDbContext("TestFindByToken");
        SeedData(context);

        var repository = new SessionRepository(context);
        var expected = new Session
        {
            RoleID = Guid.NewGuid(),
            Token = Guid.NewGuid(),
            UserID = Guid.NewGuid()
        };

        repository.AddSession(expected);
        context.SaveChanges();

        Session? result = repository.FindByToken(expected.Token);

        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Token, result.Token);
    }
}
