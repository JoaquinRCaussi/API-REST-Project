using BusinessLogic.Validators;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/validators")]
public class ValidatorController : ControllerBase
{
    private readonly ValidatorService _validatorService;

    public ValidatorController(ValidatorService validatorService)
    {
        _validatorService = validatorService;
    }

    [HttpGet]
    public IActionResult LoadValidators()
    {
        var validators = _validatorService.ChargeValidators();
        return Ok(validators);
    }
}
