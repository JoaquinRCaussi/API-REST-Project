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

    [TestMethod]
    public void GetHomeMembersTest()
    {
        var homeId = Guid.NewGuid();
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Snow",
                Email = "mail@mail.com",
                Password = "password@123"
            }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomeMembers(homeId)).Returns(users);

        var result = _homeLogic.GetHomeMembers(homeId);

        result.Should().BeEquivalentTo(users);

    }
    
    [TestMethod]
    public void AddMemberTest()
    {
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            HomeOwner = Guid.NewGuid(),
            Members = new List<Guid> { Guid.NewGuid() },
            MemberCount = 5,
            Devices = "asd"
        };

        _homeRepositoryMock?.Setup(x => x.AddMember(homeId, userId)).Returns(home);

        var result = _homeLogic?.AddMember(homeId, userId);

        result.Should().BeEquivalentTo(home);
    }
    
    [TestMethod]
    public void GetHomeTest()
    {
        var homeId = Guid.NewGuid();
        var home = new Home
        {
            Id = homeId,
            Location = "Home",
            HomeOwner = Guid.NewGuid(),
            Members = new List<Guid> { Guid.NewGuid() },
            MemberCount = 5,
            Devices = "asd"
        };

        _homeRepositoryMock?.Setup(x => x.GetHome(homeId)).Returns(home);

        var result = _homeLogic?.GetHome(homeId);

        result.Should().BeEquivalentTo(home);
    }
    
    [TestMethod]
    public void GetHomesByUserTest()
    {
        var userId = Guid.NewGuid();
        var homes = new List<Home>
        {
            new Home
            {
                Id = Guid.NewGuid(),
                Location = "Home",
                HomeOwner = userId,
                Members = new List<Guid> { Guid.NewGuid() },
                MemberCount = 5,
                Devices = "asd"
            }
        };

        _homeRepositoryMock?.Setup(x => x.GetHomesByUser(userId)).Returns(homes);

        var result = _homeLogic?.GetHomesByUser(userId);

        result.Should().BeEquivalentTo(homes);
    }
}
