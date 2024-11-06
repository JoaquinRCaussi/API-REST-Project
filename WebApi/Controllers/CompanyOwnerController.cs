using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("api/company-owner")]
[AuthenticationFilter]
public class CompanyOwnerController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public CompanyOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    [AuthorizationFilter("CanCreateCompanyOwner")]
    public IActionResult CreateCompanyOwner([FromBody] CompanyOwnerRequest user)
    {
        User companyOwner = _userLogic.CreateCompanyOwner(user.ToArgs());
        return CreatedAtAction(nameof(CreateCompanyOwner), new { id = companyOwner.Id }, companyOwner);
    }
}
