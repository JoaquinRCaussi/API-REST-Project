using System.Diagnostics.CodeAnalysis;
using Domain;
using IDataAccess;
using LogicInterface;
using Moq;
using FluentAssertions;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class UserLogicTest
{
    private Mock<IUserRepository>? _userRepositoryMock;
    private IUserLogic? _userLogic;

    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userLogic = new UserLogic(_userRepositoryMock.Object);
    }

    [TestMethod]
    public void GetUsersTest()
    {
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
        
        _userRepositoryMock.Setup(x => x.GetUsers()).Returns(users);
        
        var result = _userLogic.GetUsers();
        
        result.Should().BeEquivalentTo(users);
    }

    [TestMethod]
    public void CreateAdminTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.CreateAdmin(user)).Returns(user);

        var result = _userLogic.CreateAdmin(user);
        
        result.Should().BeEquivalentTo(user);
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.LastName.Should().Be(user.LastName);
        result.Email.Should().Be(user.Email);
        result.Password.Should().Be(user.Password);
        result.Role.Should().Be(user.Role);
    }

    [TestMethod]
    public void CreateHomeOwnerTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.CreateHomeOwner(user)).Returns(user);

        var result = _userLogic.CreateHomeOwner(user);

        result.Should().BeEquivalentTo(user);
        
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.LastName.Should().Be(user.LastName);
        result.Email.Should().Be(user.Email);
        result.Password.Should().Be(user.Password);
        result.Role.Should().Be(user.Role);
    }

    [TestMethod]
    public void CreateCompanyOwnerTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.CreateCompanyOwner(user)).Returns(user);

        var result = _userLogic.CreateCompanyOwner(user);

        result.Should().BeEquivalentTo(user);

        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.LastName.Should().Be(user.LastName);
        result.Email.Should().Be(user.Email);
        result.Password.Should().Be(user.Password);
        result.Role.Should().Be(user.Role);
    }

    [TestMethod]
    public void GetUserTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.GetUser(user.Id)).Returns(user);
        
        var result = _userLogic.GetUser(user.Id);
        
        result.Should().BeEquivalentTo(user);
        
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.LastName.Should().Be(user.LastName);
        result.Email.Should().Be(user.Email);
        result.Password.Should().Be(user.Password);
        result.Role.Should().Be(user.Role);
    }


}
