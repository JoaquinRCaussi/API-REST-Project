using BusinessLogic.Validators;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValidatorController : ControllerBase
{
    private readonly ValidatorService _validatorService;

    public ValidatorController(ValidatorService validatorService)
    {
        _validatorService = validatorService;
    }

    [HttpGet("load-validators")]
    public IActionResult LoadValidators()
    {
        var validators = _validatorService.ChargeValidators();
        return Ok(validators);
    }

    [HttpGet("get-validator/{index}")]
    public IActionResult GetValidator(int index)
    {
        try
        {
            var validator = _validatorService.GetValidator(index);
            return Ok(validator);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-validator-by-name/{name}")]
    public IActionResult GetValidatorByName(string name)
    {
        try
        {
            var validator = _validatorService.GetValidatorByName(name);
            return Ok(validator);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
