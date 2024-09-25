using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions; 
using Domain;
using LogicInterface;
using WebApi.Controllers;
using WebApi.Models;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CompanyOwnerControllerTest
{
    private CompanyOwnerController? _controller;

    [TestMethod]
    public void CreateCompanyOwner_WhenAllPropertiesOk()
    {
        var user = new User("John", "Doe", "mail@mail.com", "123456@asd");

        var companyOwnerRequest = new CompanyOwnerRequest(user);

        var companyOwnerLogic = new Mock<IUserLogic>(MockBehavior.Strict);
        companyOwnerLogic.Setup(x => x.CreateCompanyOwner(It.IsAny<User>()))
            .Returns(companyOwnerRequest.ToArgs());

        _controller = new CompanyOwnerController(companyOwnerLogic.Object);

        IActionResult act = _controller.CreateCompanyOwner(user);
        var companyOwnerResponse = new CompanyOwnerResponse(companyOwnerRequest.ToArgs());
        var expected = new OkObjectResult(companyOwnerResponse);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddCompanyToCompanyOwner_WhenAllPropertiesOk()
    {
        var company = new Company("name", "aRUT", "apath");
        var user = new User("John", "Doe", "mail@mail.com", "123456@asd");

        var addCompanyToOwnerRequest = new AddCompanyToOwnerRequest(user, company);
        var expectedResponse = new AddCompanyToOwnerResponse(user, company);

        var companyOwnerLogic = new Mock<IUserLogic>(MockBehavior.Strict);

        companyOwnerLogic.Setup(x => x.AddCompanyToCompanyOwner(It.IsAny<User>(), It.IsAny<Company>()))
            .Returns(user);

        _controller = new CompanyOwnerController(companyOwnerLogic.Object);

        var act = _controller.AddCompanyToCompanyOwner(addCompanyToOwnerRequest) as OkObjectResult;

        var returnedResponse = act.Value as AddCompanyToOwnerResponse;
        returnedResponse.Should().BeEquivalentTo(expectedResponse);
    }
}
