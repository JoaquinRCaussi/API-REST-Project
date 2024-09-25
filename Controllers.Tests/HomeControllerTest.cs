using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Controllers.Tests;

[TestClass]
public class HomeControllerTest
{
    [TestMethod]
    public void CreateHome_WhenAllPropertiesOk()
    {
        var home = new Home("location",5, "device", "homeowner");
        
        var homeRequest = new HomeRequest(home);
        var homeLogic = new Mock<IHomeLogic>(MockBehavior.Strict);
        homeLogic.Setup(x => x.CreateHome(It.IsAny<Home>())).Returns(home.ToArgs());
        
        var controller = new HomeController(homeLogic.Object);
        
        var act = controller.CreateHome(home);
        
        var homeResponse = new HomeResponse(homeRequest.ToArgs());
        var expected = new OkObjectResult(homeResponse);
        
        act.Should().BeEquivalentTo(expected);
    }
    
}
