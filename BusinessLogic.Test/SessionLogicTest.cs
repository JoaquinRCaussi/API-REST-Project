using Domain;
using IBusinessLogic;
using IDataAccess;
using FluentAssertions;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class SessionLogicTest
{
    private Mock<ISessionRepository>? _sessionRepositoryMock;
    private Mock<IUserRepository>? _userRepositoryMock;
    private ISessionLogic? _sessionLogic;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _sessionLogic = new SessionLogic(_userRepositoryMock.Object,_sessionRepositoryMock.Object);
    }

    [TestMethod]
    public void AuthenticateTest()
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
        
        var result = _sessionLogic.Authenticate(user.Email, user.Password);
        
        result.UserId.Should().Be(user.Id);
        result.RoleId.Should().Be(user.Role);
        
        _sessionRepositoryMock.Verify(x => x.AddSession(It.IsAny<Session>()), Times.Once);
    }

    [TestMethod]
    public void GetCurrentUserTest()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var session = new Session
        {
            UserID = user.Id,
            RoleID = user.Role
        };
        
        _sessionRepositoryMock.Setup(x => x.FindByToken(It.IsAny<Guid>())).Returns(session);
        
        _userRepositoryMock.Setup(x => x.GetUser(user.Id)).Returns(user);
        
        var result = _sessionLogic.GetCurrentUser(Guid.NewGuid());
        
        result.Should().BeEquivalentTo(user);
        
        _sessionRepositoryMock.Verify(x => x.FindByToken(It.IsAny<Guid>()), Times.Once);
    }

    [TestMethod]
    public void GetCurrentUserTest_WithoutToken()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@asdas.com",
            Password = "password@123"
        };

        _sessionLogic.GetCurrentUser(null).Should().BeNull();
        
        _sessionRepositoryMock.Verify(x => x.FindByToken(It.IsAny<Guid>()), Times.Never);
        
        _userRepositoryMock.Verify(x => x.GetUser(It.IsAny<Guid>()), Times.Never);
        
        _sessionLogic.GetCurrentUser(null).Should().BeNull();
    }
    
    [TestMethod]
    public void GetCurrentUserTest_InvalidToken()
    {
        _sessionRepositoryMock.Setup(x => x.FindByToken(It.IsAny<Guid>())).Returns((Session?)null);
        
        Assert.ThrowsException<Exception>(() => _sessionLogic.GetCurrentUser(Guid.NewGuid()));
        
        _userRepositoryMock.Verify(x => x.GetUser(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    public void AuthenticateTest_InvalidEmail()
    {
        _userRepositoryMock.Setup(x => x.FindByMail(It.IsAny<string>())).Returns((User?)null);

        Assert.ThrowsException<Exception>(() => _sessionLogic.Authenticate("adasdjasdj@masdjasjjasd.com", "password"));

        _sessionRepositoryMock.Verify(x => x.AddSession(It.IsAny<Session>()), Times.Never);
        
        _userRepositoryMock.Verify(x => x.FindByMail(It.IsAny<string>()), Times.Once);
    }

}
