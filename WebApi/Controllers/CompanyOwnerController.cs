using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/company-owner")]
public class CompanyOwnerController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public CompanyOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateCompanyOwner([FromBody] CompanyOwnerRequest user)
    {
        User companyOwner = _userLogic.CreateCompanyOwner(user.ToArgs());
        return Ok(companyOwner);
    }

    [HttpPut]
    public IActionResult AddCompanyToCompanyOwner(AddCompanyToOwnerRequest request)
    {
        User user = _userLogic.AddCompanyToCompanyOwner(request.CompanyOwner, request.Company);

        var response = new AddCompanyToOwnerResponse(user, request.Company);

        return Ok(response);
    }
}
