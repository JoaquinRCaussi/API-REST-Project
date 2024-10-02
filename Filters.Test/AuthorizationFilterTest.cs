using System.Diagnostics.CodeAnalysis;
using System.Net;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
                    PermissionKeys = new List<PermissionKey>
                    {
                        new PermissionKey { Value = "some-permission" }
                    }
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
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Role = new Role
                {
                    PermissionKeys = new List<PermissionKey>
                    {
                        new PermissionKey { Value = "required-permission" }
                    }
                }
            };
            var context = CreateAuthorizationFilterContext(user);
            _filter = new AuthorizationFilterAttribute("required-permission");

            // Act
            _filter.OnAuthorization(context);

            // Assert
            context.Result.Should().BeNull(); // No result means access is allowed
        }

        private AuthorizationFilterContext CreateAuthorizationFilterContext(object? userLogged)
        {
            var httpContext = new DefaultHttpContext();
            if (userLogged != null)
            {
                httpContext.Items[0] = userLogged;
            }

            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
        }
    }
