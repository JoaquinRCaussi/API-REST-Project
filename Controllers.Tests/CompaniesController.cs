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
public class CompaniesController
{
    [TestMethod]
    public void CreateCompany_WhenAllPropertiesOk()
    {
        // Arrange
        var company = new CompanyRequest("name", "aRUT", "apath");
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        companyLogic.Setup(x => x.CreateCompany(It.IsAny<Company>())).Returns(company.ToArgs());

        // Act
        var controller = new CompanyController(companyLogic.Object);
        IActionResult act = controller.CreateCompany(company);
        var companyResponse = new CompanyResponse(company.ToArgs());
        var expected = new OkObjectResult(companyResponse);
        //Assert
        act.Should().BeEquivalentTo(expected);
    }
}
