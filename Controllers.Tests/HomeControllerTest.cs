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

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id};

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(homeRequest.ToArgs());

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.CreateHome(homeRequest);

        var homeRequestObject = homeRequest.ToArgs();
        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            Devices = homeRequestObject.Devices,
            HomeOwner = homeRequestObject.HomeOwner
        };
        
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

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id};

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomes()).Returns(homes);

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.GetHomes();

        var homeRequestObject = homeRequest.ToArgs();
        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            Devices = homeRequestObject.Devices,
            HomeOwner = homeRequestObject.HomeOwner
        };
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

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id};

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomesByUser(It.IsAny<Guid>())).Returns(homes);

        var controller = new HomeController(homeLogic.Object);

        IActionResult act = controller.GetHomeByUser(user.Id);

        var homeRequestObject = homeRequest.ToArgs();
        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            Devices = homeRequestObject.Devices,
            HomeOwner = homeRequestObject.HomeOwner
        };
        var expected = new OkObjectResult(new List<HomeResponse> { homeResponse });

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHome_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id};

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };
        
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHome(It.IsAny<Guid>())).Returns(home);
        
        var controller = new HomeController(homeLogic.Object);
        
        IActionResult act = controller.GetHome(home.Id);
        
        var homeRequestObject = homeRequest.ToArgs();
        var homeResponse = new HomeResponse
        {
            Location = homeRequestObject.Location,
            MemberCount = homeRequestObject.MemberCount,
            Devices = homeRequestObject.Devices,
            HomeOwner = homeRequestObject.HomeOwner
        };
        
        var expected = new OkObjectResult(homeResponse);
        
        act.Should().BeEquivalentTo(expected);
    }
}
