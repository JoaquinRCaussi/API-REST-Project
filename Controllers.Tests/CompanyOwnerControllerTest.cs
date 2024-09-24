using System.Diagnostics.CodeAnalysis;
using Domain;
using WebApi.Models;
using FluentAssertions;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
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
            User user = new User("John", "Doe", "mail@mail.com", "123456@asd");
            
            Mock<IUserLogic> companyOwnerLogic = new Mock<IUserLogic>(MockBehavior.Strict);
            companyOwnerLogic.Setup(x => x.CreateCompanyOwner(It.IsAny<User>()))
                .Returns(user);
            
            _controller = new CompanyOwnerController(companyOwnerLogic.Object);
            
            var act = _controller.CreateCompanyOwner(user);
            var companyOwnerResponse = new User(user.Name, user.LastName, user.Email, user.Password);
            var expected = new OkObjectResult(companyOwnerResponse);
            
            act.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void AddCompanyToCompanyOwner_WhenAllPropertiesOk()
        {
            Company company = new Company("name", "aRUT", "apath");
            User user = new User("John", "Doe", "mail@mail.com", "123456@asd");

            var addCompanyToOwnerRequest = new AddCompanyToOwnerRequest(user, company);
            var expectedResponse = new AddCompanyToOwnerResponse(user, company);
            
            Mock<IUserLogic> companyOwnerLogic = new Mock<IUserLogic>(MockBehavior.Strict);
            
            companyOwnerLogic.Setup(x => x.AddCompanyToCompanyOwner(It.IsAny<User>(), It.IsAny<Company>()))
                .Returns(user);
            
            _controller = new CompanyOwnerController(companyOwnerLogic.Object);
            
            var act = _controller.AddCompanyToCompanyOwner(addCompanyToOwnerRequest) as OkObjectResult;
            
            var returnedResponse = act.Value as AddCompanyToOwnerResponse;
            returnedResponse.Should().BeEquivalentTo(expectedResponse);
        }

}
