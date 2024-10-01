using Domain;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
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
        _sessionLogic = new SessionLogic(_userRepositoryMock.Object);
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
