using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CompanyOwnerController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public CompanyOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateCompanyOwner(User user)
    {
        var companyOwner = _userLogic.CreateCompanyOwner(user);
        return Ok(companyOwner);
    }
}
