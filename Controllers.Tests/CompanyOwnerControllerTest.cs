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
public class CompanyOwnerControllerTest
{
    private CompanyOwnerController? _controller;

    [TestMethod]
    public void CreateCompanyOwner_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var companyOwnerRequest = new CompanyOwnerRequest
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password
        };

        var companyOwnerLogic = new Mock<IUserLogic>(MockBehavior.Strict);
        companyOwnerLogic.Setup(x => x.CreateCompanyOwner(It.IsAny<User>()))
            .Returns(companyOwnerRequest.ToArgs());

        _controller = new CompanyOwnerController(companyOwnerLogic.Object);

        IActionResult act = _controller.CreateCompanyOwner(companyOwnerRequest);
        var companyOwnerResponse = new CompanyOwnerResponse
        {
            Name = companyOwnerRequest.Name,
            LastName = companyOwnerRequest.LastName,
            Email = companyOwnerRequest.Email
        };
        var expected = new OkObjectResult(companyOwnerResponse);

        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddCompanyToCompanyOwner_WhenAllPropertiesOk()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };
        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "RUT", Logo = "123456789", Owner = user };

        

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
