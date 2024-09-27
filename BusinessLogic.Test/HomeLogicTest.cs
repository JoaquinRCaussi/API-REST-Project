using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IDataAccess;
using LogicInterface;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeLogicTest
{
    private Mock<IHomeRepository>? _homeRepositoryMock;
    private IHomeLogic? _homeLogic;
    
    [TestInitialize]
    public void Initialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>();
        _homeLogic = new HomeLogic(_homeRepositoryMock.Object);
    }
    
    [TestMethod]
    public void GetHomesTest()
    {
        var homes = new List<Home>
        {
            new Home
            {
                Id = Guid.NewGuid(),
                Location = "Home",
                HomeOwner = Guid.NewGuid(),
                Members = new List<Guid> { Guid.NewGuid() },
                MemberCount = 5,
                Devices = "asd"
            }
        };
        
        _homeRepositoryMock?.Setup(x => x.GetHomes()).Returns(homes);
        
        var result = _homeLogic.GetHomes();
        
        result.Should().BeEquivalentTo(homes);
    }
    
    [TestMethod]
    public void CreateHomeTest()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "Home",
            HomeOwner = Guid.NewGuid(),
            Members = new List<Guid> { Guid.NewGuid() },
            MemberCount = 5,
            Devices = "asd"
        };
        
        _homeRepositoryMock?.Setup(x => x.CreateHome(home)).Returns(home);
        
        var result = _homeLogic.CreateHome(home);
        
        result.Should().BeEquivalentTo(home);
    }
}
