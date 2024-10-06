using System.Diagnostics.CodeAnalysis;
using System.Net;
using Domain;
using FluentAssertions;
using IDataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Filters;

namespace Filters.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class AuthorizationFilterAttributeTest
{
    private AuthorizationFilterAttribute? _filter;

    [TestInitialize]
    public void Setup()
    {
        _filter = new AuthorizationFilterAttribute();
    }


    [TestMethod]
    public void OnAuthorization_UserWithoutPermission_ReturnsForbidden()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Role = new Role
            {
                PermissionKeys =
                    [
                        new PermissionKey { Value = "some-permission" }
                    ]
            }
        };
        var context = CreateAuthorizationFilterContext(user);
        _filter = new AuthorizationFilterAttribute("required-permission");

        // Act
        _filter.OnAuthorization(context);

        // Assert
        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Forbidden",
            Message = "Missing permission required-permission"
        })
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        });
    }

    [TestMethod]
    public void OnAuthorization_UserWithPermission_AllowsAccess()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Role = new Role
            {
                PermissionKeys =
                    [
                        new PermissionKey { Value = "required-permission" }
                    ]
            }
        };
        var context = CreateAuthorizationFilterContext(user);
        _filter = new AuthorizationFilterAttribute("required-permission");

        _filter.OnAuthorization(context);

        context.Result.Should().BeNull();
    }

    private AuthorizationFilterContext CreateAuthorizationFilterContext(object? userLogged, Mock<IHomeRepository>? homeRepositoryMock = null)
    {
        var httpContext = new DefaultHttpContext();
        if (userLogged != null)
        {
            httpContext.Items[0] = userLogged;
        }

        if (homeRepositoryMock != null)
        {
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(homeRepositoryMock.Object)
                .BuildServiceProvider();
        }

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }


    [TestMethod]
    public void OnAuthorization_UserWithoutRequiredPermission_ReturnsForbidden()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Role = new Role
            {
                PermissionKeys =
                [
                    new PermissionKey { Value = "other-permission" }
                ]
            }
        };
        var context = CreateAuthorizationFilterContext(user);
        _filter = new AuthorizationFilterAttribute("required-permission");

        _filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Forbidden",
            Message = "Missing permission required-permission"
        })
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        });
    }

    [TestMethod]
    public void OnAuthorization_HardwareIdHasIncorrectType_ReturnsForbidden()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe"
        };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "Some location",
            Latitude = "123",
            Longitude = "456",
            MemberCount = 5,
            Devices =
            [
                new HomeDevice
                {
                    HardwareId = Guid.NewGuid(),
                    Device = new Device { DeviceType = DeviceType.Camera } // Incorrect type
                }
            ]
        };

        var context = CreateAuthorizationFilterContext(user);
        context.RouteData.Values["homeId"] = home.Id.ToString();
        context.RouteData.Values["hardwareId"] = home.Devices.First().HardwareId.ToString();

        context.HttpContext.Request.Path = "/home/" + home.Id + "/sensor/" + home.Devices.First().HardwareId;

        var homeRepository = new Mock<IHomeRepository>();
        homeRepository.Setup(repo => repo.GetHome(home.Id)).Returns(home);

        context.HttpContext.RequestServices = new MockServiceProvider(homeRepository.Object);
        _filter = new AuthorizationFilterAttribute();

        _filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Forbidden",
            Message = "HardwareId does not have the correct type"
        })
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        });
    }

    [TestMethod]
    public void OnAuthorization_HomeNotFound_ReturnsForbidden()
    {
        var user = new User { Id = Guid.NewGuid() };
        var context = CreateAuthorizationFilterContext(user);
        context.RouteData.Values["homeId"] = Guid.NewGuid().ToString(); // ID que no existe
        _filter = new AuthorizationFilterAttribute("required-permission");

        var homeRepository = new Mock<IHomeRepository>();
        homeRepository.Setup(repo => repo.GetHome(It.IsAny<Guid>())).Returns((Home)null);

        context.HttpContext.RequestServices = new MockServiceProvider(homeRepository.Object);

        _filter.OnAuthorization(context);

        context.Result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        context.Result.Should().BeEquivalentTo(new ObjectResult(new
        {
            InnerCode = "Forbidden",
            Message = "Missing permission required-permission"
        })
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        });
    }

    [TestMethod]
    public void OnAuthorization_UserIsOwner_AllowsAccess()
    {

        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            HomeOwner = userId
        };
        var context = CreateAuthorizationFilterContext(user);
        context.RouteData.Values["homeId"] = home.Id.ToString();
        _filter = new AuthorizationFilterAttribute("IsOwner");

        var homeRepository = new Mock<IHomeRepository>();
        homeRepository.Setup(repo => repo.GetHome(home.Id)).Returns(home);

        context.HttpContext.RequestServices = new MockServiceProvider(homeRepository.Object);

        // Act
        _filter.OnAuthorization(context);

        // Assert
        context.Result.Should().BeNull(); // No hay resultado, lo que significa que se permite el acceso
    }

    [TestMethod]
    public void MemberHasPermission_RequiredPermissionIsNull_ReturnsTrue()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings =
            [
                new MemberSetting { UserId = Guid.NewGuid(), Permissions = [] }
            ]
        };
        var user = new User { Id = home.MemberSettings.First().UserId };

        var result = _filter.MemberHasPermission(home, user, null);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void MemberHasPermission_HomeIsNull_ReturnsFalse()
    {
        var user = new User { Id = Guid.NewGuid() };

        var result = _filter.MemberHasPermission(null, user, "required-permission");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberHasPermission_UserIsNull_ReturnsFalse()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings =
            [
                new MemberSetting { UserId = Guid.NewGuid(), Permissions = [] }
            ]
        };

        var result = _filter.MemberHasPermission(home, null, "required-permission");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberHasPermission_HomeMemberSettingsIsNull_ReturnsFalse()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings = null
        };
        var user = new User { Id = Guid.NewGuid() };

        var result = _filter.MemberHasPermission(home, user, "required-permission");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberHasPermission_MemberDoesNotExist_ReturnsFalse()
    {
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings =
            [
                new MemberSetting { UserId = Guid.NewGuid(), Permissions = [] }
            ]
        };
        var user = new User { Id = Guid.NewGuid() };

        var result = _filter.MemberHasPermission(home, user, "required-permission");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberHasPermission_MemberWithoutPermissions_ReturnsFalse()
    {
        var userId = Guid.NewGuid();
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings =
            [
                new MemberSetting { UserId = userId, Permissions = [] }
            ]
        };
        var user = new User { Id = userId };

        var result = _filter.MemberHasPermission(home, user, "required-permission");

        result.Should().BeFalse();
    }

    [TestMethod]
    public void MemberHasPermission_MemberWithRequiredPermission_ReturnsTrue()
    {
        var userId = Guid.NewGuid();
        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "123",
            Latitude = "123",
            Longitude = "123",
            MemberCount = 5,
            MemberSettings =
            [
                new MemberSetting
                {
                    UserId = userId,
                    Permissions =
                    [
                        new Permission { Value = "required-permission" }
                    ]
                }
            ]
        };
        var user = new User { Id = userId };

        var result = _filter.MemberHasPermission(home, user, "required-permission");

        result.Should().BeTrue();
    }




    public class MockServiceProvider : IServiceProvider
    {
        private readonly IHomeRepository _homeRepository;

        public MockServiceProvider(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IHomeRepository))
            {
                return _homeRepository;
            }

            return null;
        }
    }



}
