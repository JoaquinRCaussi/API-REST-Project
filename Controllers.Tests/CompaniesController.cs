using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class CompaniesController
{
    
    [TestInitialize]
    public void TestInitialize()
    {
        var anUser = new User { 
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"};
        
    }
    
    [TestMethod]
    public void CreateCompany_WhenAllPropertiesOk()
    {
        // Arrange
        var company = new CompanyRequest("name", "aRUT", "apath");
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        companyLogic.Setup(x => x.CreateCompany(It.IsAny<Company>())).Returns(company.ToArgs());
        var controller = new CompanyController(companyLogic.Object);
        
        // Act
        IActionResult act = controller.CreateCompany(company);
        var companyResponse = new CompanyResponse(company.ToArgs());
        var expected = new OkObjectResult(companyResponse);
        //Assert
        act.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetCompaniesTestOk()
    {
        var aCompany = new Company{
            Name = "name",
            RUT = "aRUT",
            Logo = "apath"
        };
        var companyLogic = new Mock<ICompanyLogic>(MockBehavior.Strict);
        var companies = new List<Company>
        {
            aCompany
        };
        companyLogic.Setup(x => x.GetCompanies()).Returns(companies);
        
        var controller = new CompanyController(companyLogic.Object);
        
        IActionResult act = controller.GetCompanies();
        var expected = new OkObjectResult(companies.Select(x => new CompanyResponse(x)).ToList());
        act.Should().BeEquivalentTo(expected);
    }
}
