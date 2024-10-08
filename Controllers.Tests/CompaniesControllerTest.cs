using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CompaniesControllerTest
{

    [TestMethod]
    public void CreateCompany_WhenAllPropertiesOk()
    {
        // Arrange
        var owner = new User()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Doe",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items[0] = owner;

        var companyRequest = new CompanyRequest("name", "aRUT", "apath");

        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);

        var createdCompany = companyRequest.ToArgs(owner); // Suponiendo que ToArgs retorna el objeto Company.
        companyLogic.Setup(x => x.CreateCompany(It.IsAny<Company>())).Returns(createdCompany);

        var controller = new CompanyController(companyLogic.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        IActionResult act = controller.CreateCompany(companyRequest);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateCompany),
            nameof(CompanyController).Replace("Controller", ""),
            new { id = createdCompany.Id },
            new CompanyResponse(createdCompany)
        );

        // Assert
        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.ControllerName)
            .Excluding(x => x.RouteValues));
    }


    [TestMethod]
    public void GetCompanies_AllowFilterByCompanyName()
    {
        var aCompany = new Company
        {
            Name = "name",
            RUT = "aRUT",
            Logo = "apath",
            Owner = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "mail@mail.com",
                Password = "password@123"
            }
        };
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        var companies = new List<Company> { aCompany };

        companyLogic.Setup(x => x.GetCompanies(aCompany.Name, aCompany.Owner.Name)).Returns(companies);

        var controller = new CompanyController(companyLogic.Object);

        IActionResult act = controller.GetCompanies("name", aCompany.Owner.Name);

        var expected = new OkObjectResult(companies.Select(x => new CompanyResponse(x)
        {
            OwnerName = x.Owner.Name,
            OwnerEmail = x.Owner.Email
        }).ToList());

    }

    [TestMethod]
    public void GetCompanies_AllowFilterByOwnersName()
    {
        var aCompany = new Company
        {
            Name = "name",
            RUT = "aRUT",
            Logo = "apath",
            Owner = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                LastName = "Doe",
                Email = "mail@mail.com",
                Password = "password@123"
            }
        };

        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        var companies = new List<Company>
        {
        };
        companyLogic.Setup(x => x.GetCompanies(aCompany.Name, aCompany.Owner.Name)).Returns(companies);


    }
}
