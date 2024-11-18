using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
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

        var createdCompany = companyRequest.ToArgs(owner);
        companyLogic.Setup(x => x.CreateCompany(It.IsAny<Company>())).Returns(createdCompany);

        var controller = new CompanyController(companyLogic.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        IActionResult act = controller.CreateCompany(companyRequest);

        var expected = new CreatedAtActionResult(
            nameof(controller.CreateCompany),
            nameof(CompanyController).Replace("Controller", ""),
            new { id = createdCompany.Id },
            new CompanyResponse(createdCompany)
        );

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
        var totalResults = 1;

        companyLogic.Setup(x => x.GetCompanies(aCompany.Name, aCompany.Owner.Name, 1, 10))
                    .Returns((companies, totalResults));

        var controller = new CompanyController(companyLogic.Object);

        IActionResult act = controller.GetCompanies("name", aCompany.Owner.Name, 1, 10);

        var okResult = act as OkObjectResult;
        okResult.Should().NotBeNull();

        var expectedResponse = new
        {
            TotalResults = totalResults,
            PageNumber = 1,
            PageSize = 10,
            Companies = companies.Select(c => new CompanyResponse(c)).ToList()
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.ComparingByMembers<object>());
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
        var companies = new List<Company>();
        var totalResults = 0;

        companyLogic.Setup(x => x.GetCompanies("name", "John", 1, 10))
                    .Returns((companies, totalResults));

        var controller = new CompanyController(companyLogic.Object);

        IActionResult act = controller.GetCompanies("name", "John", 1, 10);

        // Verificar que el resultado sea un OkObjectResult con una lista vacía
        var okResult = act as OkObjectResult;
        okResult.Should().NotBeNull();

        var expectedResponse = new
        {
            totalResults = 0,
            pageNumber = 1,
            pageSize = 10,
            companies = new List<CompanyResponse>()
        };

        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public void GetCompanies_ShouldReturnNoContentWhenNoCompaniesFound()
    {
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);

        // Configurar para lanzar la excepción si no se encuentran compañías
        companyLogic.Setup(x => x.GetCompanies(null, null, 1, 10))
                    .Throws(new EmptyException("No companies found"));

        var controller = new CompanyController(companyLogic.Object);

        // Verificar que el resultado sea NoContent cuando no hay compañías
        IActionResult result = controller.GetCompanies(null, null, 1, 10);

        result.Should().BeOfType<NoContentResult>();

        companyLogic.VerifyAll();
    }
}
