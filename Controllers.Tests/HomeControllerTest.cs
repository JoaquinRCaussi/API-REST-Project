using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeControllerTest
{
    [TestMethod]
    public void CreateHome_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home("location", 5, "device", user);

        var homeRequest = new HomeRequest(home);
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(homeRequest.ToArgs());

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.CreateHome(homeRequest);

        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(homeResponse);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomes_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };
        
        var home = new Home("location", 5, "device", user);

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest(home);
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomes()).Returns(homes);

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.GetHomes();

        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(new List<HomeResponse> { homeResponse });

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomesByUser_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };
        
        var home = new Home("location", 5, "device", user);

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest(home);

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomesByUser(It.IsAny<Guid>())).Returns(homes);

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.GetHomeByUser(user.Id);

        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(new List<HomeResponse> { homeResponse });

        act.Should().BeEquivalentTo(expected);
    }
}
