using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

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

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            Devices = "device",
            HomeOwner = user.Id
        };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(homeRequest.ToArgs());

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

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

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id };

        var homes = new List<Home> { home };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHomes()).Returns(homes);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

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

        var home = new Home { Id = Guid.NewGuid(), Location = "location", MemberCount = 5, Devices = "device", HomeOwner = user.Id };

        var homeRequest = new HomeRequest
        {
            Location = home.Location,
            MemberCount = home.MemberCount,
            Devices = home.Devices,
            HomeOwner = home.HomeOwner
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHome(It.IsAny<Guid>())).Returns(home);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

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

    [TestMethod]
    public void GetHomeMembers_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var users = new List<User> { user };

        var home = new Home
        {
            Id = Guid.NewGuid(),
            Location = "location",
            MemberCount = 5,
            Devices = "device",
            HomeOwner = user.Id,
            Members = users
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.GetHomeMembers(It.IsAny<Guid>())).Returns(users);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        IActionResult act = controller.GetHomeMembers(home.Id);

        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        var expectedResponse = new List<GetHomeMembersResponse>
        {
            new()
            {
                Name = user.Name,
                Email = user.Email
            }
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());

        homeLogic.Verify(x => x.GetHomeMembers(home.Id), Times.Once);
    }

    [TestMethod]
    public void AddMemberToHome_WhenAllPropertiesOk()
    {
        // Arrange
        var homeId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var addMemberRequest = new AddMemberRequest
        {
            UserId = userId.ToString()
        };

        var home = new Home
        {
            Id = homeId,
            Location = "TestLocation",
            MemberCount = 1,
            Devices = "device",
            HomeOwner = Guid.NewGuid(),
            Members = []
        };

        var memberSetting = new MemberSetting
        {
            HomeId = homeId,
            UserId = userId,
            Permissions = []
        };

        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        var memberSettingLogic = new Mock<IMemberSettingLogic>(MockBehavior.Strict);

        homeLogic.Setup(x => x.AddMember(homeId, userId)).Returns(home);
        memberSettingLogic.Setup(x => x.CreateMemberSetting(homeId, userId)).Returns(memberSetting);

        var controller = new HomeController(homeLogic.Object, memberSettingLogic.Object);

        // Act
        IActionResult act = controller.AddMemberToHome(homeId, addMemberRequest);

        // Assert
        var okResult = act as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult");

        var expectedResponse = new AddMemberResponse
        {
            Home = home,
            MemberSetting = memberSetting
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());

        // Verifica que los métodos en los mocks se hayan llamado exactamente una vez con los parámetros correctos
        homeLogic.Verify(x => x.AddMember(homeId, userId), Times.Once);
        memberSettingLogic.Verify(x => x.CreateMemberSetting(homeId, userId), Times.Once);
    }


}
