using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
[TestClass]
public class CompanyLogicTest
{

    [TestMethod]
    public void CreateCompanyTest_WhenAllPropertiesOk()
    {
        
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "Address",
            Owner = user
        };
        
        var mock = new Mock<ICompanyRepository>(MockBehavior.Strict);
        mock.Setup(x => x.CreateCompany(company)).Returns(company);
        
        // Act
        var companyLogic = new CompanyLogic(mock.Object);
        var result = companyLogic.CreateCompany(company);

        result.Should().BeEquivalentTo(company);
    }


    [TestMethod]
    public void CreateCompany_WhenTheOwnerHasAlreadyACompany()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123",
        };

        var company = new Company { Id = Guid.NewGuid(), Name = "Company", RUT = "Address", Owner = user };
        
        user.Company = company;
        user.CompanyID = company.Id;
        
        var mock = new Mock<ICompanyRepository>(MockBehavior.Strict);
        mock.Setup(x => x.CreateCompany(company)).Returns(company);
        var companyLogic = new CompanyLogic(mock.Object);
        
        // Act
        Action act = () => companyLogic.CreateCompany(company);

        act.Should().Throw<ConflictException>().WithMessage("The owner already has a company");
    }

    [TestMethod]
    public void GetCompaniesTest_WhenAllPropertiesOk()
    {
        // Arrange
        var mock = new Mock<ICompanyRepository>(MockBehavior.Strict);
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Company",
            RUT = "ARut",
            Owner = new User { Id = Guid.NewGuid(), Name = "John", LastName = "Snow", Email = "Asa@gmail.com" }
        };
        var companies = new List<Company> { company };
        mock.Setup(x => x.GetCompanies("", "")).Returns(companies);
        
        // Act
        var companyLogic = new CompanyLogic(mock.Object);
        var result = companyLogic.GetCompanies(null, null);
        
        // Assert
        result.Should().BeEquivalentTo(companies);
    }

    [TestMethod]
    public void CreateCompany_WhenPropertiesAreWrong()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            LastName = "Snow",
            Email = "mail@mail.com",
            Password = "password@123"
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "",
            RUT = "",
            Owner = user
        };
        
        var mock = new Mock<ICompanyRepository>(MockBehavior.Strict);
        mock.Setup(x => x.CreateCompany(company)).Returns(company);
        var companyLogic = new CompanyLogic(mock.Object);
        
        // Act
        Action act = () => companyLogic.CreateCompany(company);
        
        // Assert
        
        act.Should().Throw<NotValidDataException>().WithMessage("The name and RUT are required");
    }

}
