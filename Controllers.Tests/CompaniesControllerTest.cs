using System.Diagnostics.CodeAnalysis;
using System.Net;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using WebApi.Controllers;
using WebApi.Filters;
using WebApi.Models.In;
using WebApi.Models.Out;

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

        var companyRequest = new CompanyRequest("name", "aRUT", "apath", "modelo");

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

    [TestMethod]
    public void GetCompanies_ShouldReturnNoContentWhenNoCompaniesFound()
    {
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        companyLogic.Setup(x => x.GetCompanies(null, null))
            .Throws(new EmptyException("No companies found"));

        var controller = new CompanyController(companyLogic.Object);

        var context = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        var exceptionFilter = new ExceptionFilter();
        var exceptionContext = new ExceptionContext(context, new List<IFilterMetadata>())
        {
            Exception = new EmptyException("No companies found")
        };

        Action act = () => controller.GetCompanies(null, null);

        act.Should().Throw<EmptyException>();

        exceptionFilter.OnException(exceptionContext);

        var result = exceptionContext.Result as ObjectResult;
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        companyLogic.VerifyAll();
    }
}
