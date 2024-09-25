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
        var user = new User("John", "Doe", "mail@mail.com", "123456@asd");
        
        var home = new Home("location",5, "device", user);
        
        var homeRequest = new HomeRequest(home);
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(homeRequest.ToArgs());
        
        var controller = new HomeController(homeLogic.Object);
        
        var act = controller.CreateHome(homeRequest);
        
        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(homeResponse);
        
        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomes_WhenAllPropertiesOk()
    {
        var user = new User("John", "Doe", "mail@mail.com", "123456@asd");
        var home = new Home("location",5, "device", user);
        
        var homeRequest = new HomeRequest(home);
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomes()).Returns(new List<Home> {home});
        
        var controller = new HomeController(homeLogic.Object);
        
        var act = controller.GetHomes();
        
        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(new List<HomeResponse> {homeResponse});
        
        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetHomeByUser_WhenAllPropertiesOk()
    {
        var user = new User("John", "Doe", "mail@mail.com", "123456@asd");
        var home = new Home("location", 5, "device", user);

        var homeRequest = new HomeRequest(home);
        
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.GetHomeByUser(It.IsAny<User>())).Returns(new List<Home> {home});
        
        var controller = new HomeController(homeLogic.Object);
        
        var act = controller.GetHomeByUser(user);
        
        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(new List<HomeResponse> {homeResponse});
        
        act.Should().BeEquivalentTo(expected);
    }
}
