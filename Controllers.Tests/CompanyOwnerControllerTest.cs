using System.Diagnostics.CodeAnalysis;
using Domain;
using FluentAssertions;
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
}
