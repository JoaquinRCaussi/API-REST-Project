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
        var createdOwner = companyOwnerRequest.ToArgs();
        companyOwnerLogic.Setup(x => x.CreateCompanyOwner(It.IsAny<User>()))
            .Returns(createdOwner);

        _controller = new CompanyOwnerController(companyOwnerLogic.Object);

        IActionResult act = _controller.CreateCompanyOwner(companyOwnerRequest);

        var companyOwnerResponse = new CompanyOwnerResponse
        {
            Name = companyOwnerRequest.Name,
            LastName = companyOwnerRequest.LastName,
            Email = companyOwnerRequest.Email
        };

        var expected = new CreatedAtActionResult(
            nameof(_controller.CreateCompanyOwner),
            nameof(CompanyOwnerController).Replace("Controller", ""),
            new { id = createdOwner.Id },
            companyOwnerResponse
        );

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }


}
