using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Validators;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace Controllers.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class ValidatorControllerTest
{
    [TestMethod]
    public void LoadValidators_WhenValidatorsAreLoaded_ShouldReturnOkWithValidators()
    {
        var validators = new List<string> { "ValidatorA", "ValidatorB", "ValidatorC" };

        var validatorServiceMock = new Mock<ValidatorService>();
        validatorServiceMock.Setup(v => v.ChargeValidators()).Returns(validators);

        var controller = new ValidatorController(validatorServiceMock.Object);
        
        IActionResult act = controller.LoadValidators();
        
        var expected = new OkObjectResult(validators);

        act.Should().BeEquivalentTo(expected, options => options
            .ExcludingMissingMembers());
    }
}
