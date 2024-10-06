using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;

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

    [TestMethod]
    public void AddCompanyToUserTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "Address",
            Owner = user
        };

        _userRepositoryMock.Setup(x => x.AddCompanyToCompanyOwner(user, company)).Returns(user);

        var result = _userLogic.AddCompanyToCompanyOwner(user, company);

        result.Should().BeEquivalentTo(user);

        result.Company.Should().Be(user.Company);
    }

    [TestMethod]
    public void FindByMailTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.FindByMail(user.Email)).Returns(user);

        var result = _userLogic.FindByMail(user.Email);

        result.Should().BeEquivalentTo(user);

        result.Id.Should().Be(user.Id);

        _userRepositoryMock.Verify(x => x.FindByMail(user.Email), Times.Once);
    }

    [TestMethod]
    public void ExistUserTest_WhenUserExist()
    {
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.ExistUser(userId)).Returns(true);

        var result = _userLogic.ExistUser(userId);

        result.Should().BeTrue();

        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteUserTest_WhenUserExist()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.AuthenticateUser(user.Email, user.Password)).Returns(user);

        var result = _userLogic.AuthenticateUser(user.Email, user.Password);

        result.Should().BeEquivalentTo(user);

        result.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);

        _userRepositoryMock.Verify(x => x.AuthenticateUser(user.Email, user.Password), Times.Once);
    }

    [TestMethod]
    public void ExistUserTest()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.ExistUser(userId)).Returns(true);

        // Act
        var result = _userLogic.ExistUser(userId);

        // Assert
        result.Should().BeTrue();

        _userRepositoryMock.Verify(x => x.ExistUser(userId), Times.Once);
    }

    [TestMethod]
    public void DeleteUserTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@gmail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.ExistUser(user.Id)).Returns(true);
        _userRepositoryMock.Setup(x => x.DeleteUser(user.Id)).Returns(user);

        var act = _userLogic.DeleteUser(user.Id);

        act.Should().BeEquivalentTo(user);

        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteNotExistUserTest_WhenUserNotExist()
    {

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        _userRepositoryMock.Setup(x => x.ExistUser(user.Id)).Returns(false);

        var act = () => _userLogic.DeleteUser(user.Id);

        act.Should().Throw<NotValidDataException>().WithMessage("User does not exist");

        _userRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void CreateUser_WhenUserHasNoEmailValid()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@.com",
            Password = "password@123"
        };

        var act = () => _userLogic.CreateAdmin(user);

        act.Should().Throw<NotValidDataException>().WithMessage("Email is not valid");
    }


    [TestMethod]
    public void CreateUser_WhenUserHasNoNameOrPasword()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "", Password = "", LastName = "Snow", Email = "mail@mail.com" };

        var act = () => _userLogic.CreateAdmin(user);

        act.Should().Throw<NotValidDataException>().WithMessage("User data is not valid");
    }

    [TestMethod]
    public void CreateCompanyOwner_WhenUserHasNoNameOrPasword()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "",
            Password = "",
            LastName = "Snow",
            Email = "mail@mail.com"
        };
        var act = () => _userLogic.CreateCompanyOwner(user);

        act.Should().Throw<NotValidDataException>().WithMessage("User data is not valid");
    }

    [TestMethod]
    public void CreateHomeOwner_WhenUserHasNoNameOrPasword()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "",
            Password = "",
            LastName = "Snow",
            Email = "mail@mail.com"
        };
        var act = () => _userLogic.CreateHomeOwner(user);

        act.Should().Throw<NotValidDataException>().WithMessage("User data is not valid");
    }

    [TestMethod]
    public void GetNotificationsTest()
    {
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = Guid.NewGuid(),
                Event = "Event",
                UserId = userId,
                CreatedAt = DateTime.Now,
                IsRead = false,
                HardwareId = Guid.NewGuid()
            }
        };

        _userRepositoryMock.Setup(x => x.GetNotifications(userId)).Returns(notifications);

        var result = _userLogic.GetNotifications(userId);

        result.Should().BeEquivalentTo(notifications);
    }
}
