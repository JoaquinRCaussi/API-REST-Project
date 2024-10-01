using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class SessionServiceTests
{
    private Mock<IUserRepository>? _userRepositoryMock;
    private SessionService? _sessionService;


    [TestInitialize]
    public void Initialize()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _sessionService = new SessionService(_userRepositoryMock.Object);
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
        session.Token.Should().NotBeNullOrEmpty();
        session.RoleID.Should().Be(user.RoleID);
    }

    [TestMethod]
    public void GetUserByToken_ShouldReturnUser_WhenSessionExists()
    {

        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = "valid_token", RoleID = Guid.NewGuid() };
        _sessionService.AddSession(session);


        var result = _sessionService.GetUserByToken("valid_token");


        result.Should().Be(user);
    }

    [TestMethod]
    public void GetUserByToken_ShouldThrowException_WhenSessionNotFound()
    {

        Action act = () => _sessionService.GetUserByToken("invalid_token");


        act.Should().Throw<Exception>().WithMessage("Session not found");
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

        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = "existing_token", RoleID = Guid.NewGuid() };
        _sessionService.AddSession(session);


        Action act = () => _sessionService.AddSession(session);


        act.Should().Throw<InvalidOperationException>().WithMessage("A session with the same token already exists");
    }

    [TestMethod]
    public void AddSession_ShouldAddSession_WhenValidSessionProvided()
    {

        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", RoleID = Guid.NewGuid() };
        var session = new Session { User = user, Token = "new_token", RoleID = Guid.NewGuid() };


        _sessionService.AddSession(session);


        var result = _sessionService.GetUserByToken("new_token");
        result.Should().Be(user);
    }
}
