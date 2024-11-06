using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.In;
using WebApi.Models.Out;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeOwnerControllerTest
{
    [TestMethod]
    public void CreateHomeOwner_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var homeOwnerReq = new HomeOwnerRequest
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            ImagePath = user.ImagePath
        };
        var logic = new Mock<IUserLogic>(MockBehavior.Strict);
        logic.Setup(l => l.CreateHomeOwner(It.IsAny<User>())).Returns(homeOwnerReq.ToUser());

        var controller = new HomeOwnerController(logic.Object);
        IActionResult act = controller.CreateHomeOwner(homeOwnerReq);
        var homeOwnerResponse = new HomeOwnerResponse
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email
        };

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateHomeOwner),
            nameof(HomeOwnerController).Replace("Controller", ""),
            new { id = user.Id },
            new HomeOwnerResponse
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            }
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }
}
