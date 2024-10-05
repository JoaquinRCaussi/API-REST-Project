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
        var company = new CompanyRequest("name", "aRUT", "apath");
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        companyLogic.Setup(x => x.CreateCompany(It.IsAny<Company>())).Returns(company.ToArgs(owner));
        var controller = new CompanyController(companyLogic.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        IActionResult act = controller.CreateCompany(company);
        var companyResponse = new CompanyResponse(company.ToArgs(owner));
        var expected = new OkObjectResult(companyResponse);
        //Assert
        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetCompaniesTestOk()
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
            aCompany
        };
        companyLogic.Setup(x => x.GetCompanies("", "")).Returns(companies);

        var controller = new CompanyController(companyLogic.Object);

        IActionResult act = controller.GetCompanies("", "");
        var expected = new OkObjectResult(companies.Select(x => new CompanyResponse(x)).ToList());
        act.Should().BeEquivalentTo(expected);
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
        var companies = new List<Company>
        {
            aCompany
        };
        companyLogic.Setup(x => x.GetCompanies(aCompany.Name, aCompany.Owner.Name)).Returns(companies);

        var controller = new CompanyController(companyLogic.Object);

        IActionResult act = controller.GetCompanies("name", aCompany.Owner.Name);
        var expected = new OkObjectResult(companies.Select(x => new CompanyResponse(x)).ToList());
        act.Should().BeEquivalentTo(expected);
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
