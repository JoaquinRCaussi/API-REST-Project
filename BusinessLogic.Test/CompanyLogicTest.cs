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

        act.Should().Throw<ConflictException>();
    }
}
