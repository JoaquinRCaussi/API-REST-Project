using System.Diagnostics.CodeAnalysis;
using BusinessLogic.DataAccessInterfaces;
using BusinessLogic.Entities;
using FluentAssertions;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class SessionServiceTests
{
    private Mock<IUserRepository>? _userRepositoryMock;
    private Mock<ISessionRepository>? _sessionRepositoryMock;
    private SessionService? _sessionService;


    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _sessionService = new SessionService(_userRepositoryMock.Object, _sessionRepositoryMock.Object);
    }

    [TestMethod]
    public void Authenticate_ShouldThrowException_WhenUserNotFound()
    {

        var email = "test@example.com";
        var password = "password123";
        _userRepositoryMock.Setup(repo => repo.FindByMail(email)).Returns((User)null);


        Action act = () => _sessionService.Authenticate(email, password);


        act.Should().Throw<UnauthorizedAccessException>().WithMessage("Invalid email or password");
    }

    [TestMethod]
    public void Authenticate_ShouldReturnSession_WhenUserIsAuthenticated()
    {

        var email = "test@example.com";
        var password = "password123";
        var user = new User { Id = Guid.NewGuid(), Email = email, RoleID = Guid.NewGuid() };
        _userRepositoryMock.Setup(repo => repo.FindByMail(email)).Returns(user);

        var session = _sessionService.Authenticate(email, password);

        session.User.Should().Be(user);
        session.RoleID.Should().Be(user.RoleID);
    }

    [TestMethod]
    public void GetUserByToken_ShouldReturnUser_WhenSessionExists()
    {
        var token = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = token, RoleID = Guid.NewGuid() };

        _sessionService.AddSession(session);

        _sessionRepositoryMock.Setup(repo => repo.FindByToken(token)).Returns(session);
        var result = _sessionService.GetUserByToken(token);

        result.Should().Be(user);
    }

    [TestMethod]
    public void GetUserByToken_ShouldThrowException_WhenSessionNotFound()
    {
        var invalidToken = Guid.NewGuid();
        _sessionRepositoryMock.Setup(repo => repo.FindByToken(invalidToken)).Returns((Session)null);

        Action act = () => _sessionService.GetUserByToken(invalidToken);

        act.Should().Throw<Exception>().WithMessage("Invalid token or token not found");
    }

    [TestMethod]
    public void AddSession_ShouldThrowException_WhenSessionIsNull()
    {

        Action act = () => _sessionService.AddSession(null);


        act.Should().Throw<ArgumentNullException>().WithMessage("Session cannot be null*");
    }

    [TestMethod]
    public void AddSession_ShouldThrowException_WhenSessionWithSameTokenExists()
    {
        var token = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = token, RoleID = Guid.NewGuid() };
        _sessionRepositoryMock.Setup(repo => repo.FindByToken(token)).Returns(session);

        Action act = () => _sessionService.AddSession(session);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("A session with the same token already exists");
    }

    [TestMethod]
    public void AddSession_ShouldAddSession_WhenValidSessionProvided()
    {
        var token = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = token, RoleID = Guid.NewGuid() };

        _sessionService.AddSession(session);

        _sessionRepositoryMock.Setup(repo => repo.FindByToken(token)).Returns(session);
        var result = _sessionService.GetUserByToken(token);
        result.Should().Be(user);
    }
}
