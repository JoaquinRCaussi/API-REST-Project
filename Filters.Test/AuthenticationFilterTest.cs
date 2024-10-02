using System.Diagnostics.CodeAnalysis;
using System.Net;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Filters;

namespace Filters.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class AuthenticationFilterAttributeTest
{
    private Mock<ISessionService>? _sessionServiceMock;
    private AuthenticationFilterAttribute? _filter;

    [TestInitialize]
    public void Setup()
    {
        _sessionServiceMock = new Mock<ISessionService>();
        _filter = new AuthenticationFilterAttribute();
    }

    [TestMethod]
    public void OnAuthorization_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        // Arrange
        var context = CreateAuthorizationFilterContext(null);

        // Act
        _filter.OnAuthorization(context);

        // Assert
        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Unauthenticated",
            Message = "You are not authenticated"
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        });
    }

    [TestMethod]
    public void OnAuthorization_WithInvalidAuthorizationFormat_ReturnsUnauthorized()
    {
        // Arrange
        var context = CreateAuthorizationFilterContext("InvalidFormat");

        // Act
        _filter.OnAuthorization(context);

        // Assert
        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "InvalidAuthorization",
            Message = "The provided authorization header format is invalid"
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        });
    }

    [TestMethod]
    public void OnAuthorization_WithValidToken_SetsUserInHttpContext()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Name = "John Doe" };
        _sessionServiceMock.Setup(s => s.GetUserByToken("validToken")).Returns(user);

        var context = CreateAuthorizationFilterContext("Bearer validToken");
        context.HttpContext.RequestServices = CreateServiceProvider().BuildServiceProvider();

        // Act
        _filter.OnAuthorization(context);

        // Assert
        context.HttpContext.Items[0].Should().Be(user);
    }

    private AuthorizationFilterContext CreateAuthorizationFilterContext(string? authorizationHeader)
    {
        var httpContext = new DefaultHttpContext();
        if (authorizationHeader != null)
        {
            httpContext.Request.Headers["Authorization"] = authorizationHeader;
        }

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    private IServiceCollection CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_sessionServiceMock.Object);
        return services;
    }
}
