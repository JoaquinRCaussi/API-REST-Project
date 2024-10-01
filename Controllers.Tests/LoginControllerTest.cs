using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[TestClass]
public class LoginControllerTest
{
    private Mock<ISessionLogic> _sessionLogicMock = null!;
    private LoginController _loginController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _sessionLogicMock = new Mock<ISessionLogic>();
        _loginController = new LoginController(_sessionLogicMock.Object);
    }

    [TestMethod]
    public void Login_ValidCredentials_ReturnsOkResultWithToken()
    {
        var userId = Guid.NewGuid();
        var userRoleId = Guid.NewGuid();

        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var authResult = new AuthenticationResult
        {
            UserId = userId,
            RoleId = userRoleId
        };

        _sessionLogicMock
            .Setup(x => x.Authenticate(loginRequest.Email, loginRequest.Password))
            .Returns(authResult);

        var httpContext = new DefaultHttpContext();
        _loginController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var actionResult = _loginController.Login(loginRequest);
        actionResult.Should().BeOfType<OkObjectResult>();
        var result = actionResult as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);

        var response = result.Value as LoginResponse;
        response.Should().NotBeNull();
        response.Token.Should().Be(userId.ToString());

        // Verifica que la cabecera Authorization contiene el token esperado
        _loginController.Response.Headers.Should().ContainKey("Authorization");
        _loginController.Response.Headers["Authorization"].ToString().Should().Be(userId.ToString());
    }




}
